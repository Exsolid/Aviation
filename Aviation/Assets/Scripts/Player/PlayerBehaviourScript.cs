using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using AudioBuddyTool;
public class PlayerBehaviourScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody playerrb;
    [SerializeField] private Transform gunPosLeft;
    [SerializeField] private Transform gunPosRight;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject smoke;
    public HealthBar healthBar;
    [SerializeField] private PlayerInput playerInput = null;
    [SerializeField] private CharacterController controller = null;

    [Header("Settings")]
    private Vector3 movementSpeed;
    public int maxHealth;
    public int currentHealth;
    public int fuel;
    [SerializeField] private float timeBetweenFuelLoss = 3f;
    private float timeForFuelLoss;
    [SerializeField] private float defSpeed;
    private float defSpeedChange;

    public PlayerInput PlayerInput => playerInput;

    public float TimeBetweenFuelLoss { get { return timeBetweenFuelLoss; } }
    private Scaler scaler;
    private float maxDisplayHeightAtGameplay;
    private float maxDisplayWidthAtGameplay;

    [SerializeField]private float shootTiming;
    private float shootTimer;

    private float speedAdjustTimer;
    private float speedAdjustTimerChange;
    private float timer;

    private bool crashing;
    private int randomDir;

    private List<GameObject> smokes;

    // Start is called before the first frame update
    void Start()
    {
        smokes = new List<GameObject>();
        speedAdjustTimer = 0.004f;
        shootTimer = 0;
        playerrb = GetComponent<Rigidbody>();
        playerrb.useGravity = false;
        currentHealth = maxHealth+ fuel;
        healthBar.SetMaxHealth(fuel+maxHealth);
        timeForFuelLoss = Time.time + timeBetweenFuelLoss;
        movementSpeed = new Vector3(0,0,0);
        scaler = gameObject.GetComponent<Scaler>();
        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        maxDisplayWidthAtGameplay = maxDisplayHeightAtGameplay * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (!crashing)
        {
            timer += Time.deltaTime;
            shootTimer += Time.deltaTime;
            playerInput.actions["Shoot"].performed += _ => Shoot();
            Vector2 input = playerInput.actions["Movement"].ReadValue<Vector2>();
            controller.enabled = true;
            controller.Move(Time.deltaTime * movementSpeed);
            controller.enabled = false;

            // Player can't leave camera view
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, -maxDisplayWidthAtGameplay / 2 , maxDisplayWidthAtGameplay / 2 - scaler.BorderSizeRight);
            pos.z = Mathf.Clamp(pos.z, -maxDisplayHeightAtGameplay / 2 + gameObject.GetComponent<Collider>().bounds.size.z, maxDisplayHeightAtGameplay / 2 - gameObject.GetComponent<Collider>().bounds.size.z / 1.75f);
            transform.position = pos;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 35 * movementSpeed.x / defSpeed * -1);

            //FuelConsumption takes effect when time has passed
            if (timeForFuelLoss <= Time.time)
            {
                FuelConsumption(1f);
                timeForFuelLoss = Time.time + timeBetweenFuelLoss;
            }
            if (speedAdjustTimer + speedAdjustTimerChange < timer)
            {
                timer = 0;

                float dir = input.x == 0 ? (movementSpeed.x > 0 ? -0.65f : 0.65f) : input.x;
                movementSpeed.x = Mathf.Clamp(movementSpeed.x + 0.5f * dir, -defSpeed - defSpeedChange, defSpeed + defSpeedChange);
                dir = input.y == 0 ? (movementSpeed.z > 0 ? -0.5f : 0.5f) : input.y;
                movementSpeed.z = Mathf.Clamp(movementSpeed.z + 0.5f * dir, -defSpeed - defSpeedChange, defSpeed + defSpeedChange);
            }

        }

        if (currentHealth <= 0 && !crashing)
        {
            AviationEventManagerGui.Instance.GameOver();
            crashing = true;
            StartCoroutine(crash());
        }
    }

    private void Shoot()
    {
        if (shootTimer > shootTiming)
        {
            AudioBuddy.Play("gun_cut", Options.Instance.EffectVolume);
            shootTimer = 0;

            if (gunPosLeft != null && bulletPrefab != null)
            {
                //Create bullet and add the planes speed to the bullet speed
                GameObject obj = GameObject.Instantiate(bulletPrefab, gunPosLeft.transform.position, Quaternion.Euler(0, 0, 0));
                obj.layer = LayerMask.NameToLayer("Player");
            }
            if (gunPosRight != null && bulletPrefab != null)
            {
                //Create bullet and add the planes speed to the bullet speed
                GameObject obj = GameObject.Instantiate(bulletPrefab, gunPosRight.transform.position, Quaternion.Euler(0, 0, 0));
                obj.layer = LayerMask.NameToLayer("Player");
            }
        }
    }

    private void FuelConsumption(float loss)
    {
        currentHealth -= (int)loss;
        healthBar.reduceHealth((int)loss);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.reduceHealth(damage);
        GameObject obj = Instantiate(smoke, new Vector3(transform.position.x + Random.Range(-gameObject.GetComponent<Collider>().bounds.size.x / 2.75f, gameObject.GetComponent<Collider>().bounds.size.x/ 2.75f), transform.position.y - gameObject.GetComponent<Collider>().bounds.size.y, transform.position.z + gameObject.GetComponent<Collider>().bounds.size.z/5f), Quaternion.Euler(180,0,0));
        obj.transform.parent = gameObject.transform;
        smokes.Add(obj);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.tag.Equals("Collectable") && !collision.gameObject.tag.Equals("StatusChanger"))
        {
            if (collision.gameObject.tag != "Birb") AudioBuddy.Play("metal_hit", Options.Instance.EffectVolume);
            TakeDamage(2);
        }
        AviationEventManagerGui.Instance.onCollision(gameObject, collision.gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.tag.Equals("Collectable") && !collision.gameObject.tag.Equals("StatusChanger"))
        {
            if (collision.gameObject.tag != "Birb") AudioBuddy.Play("metal_hit", Options.Instance.EffectVolume);
            TakeDamage(2);
        }
        AviationEventManagerGui.Instance.onCollision(gameObject, collision.gameObject);
    }

    public void reduceSpeed()
    {
        defSpeedChange += -6;
        speedAdjustTimerChange += 0.001f;
        StartCoroutine(resetSpeed(4, 0.001f, -6));
    }

    public void increaseSpeed()
    {
        defSpeedChange += 20;
        speedAdjustTimerChange += -0.005f;
        StartCoroutine(resetSpeed(4, -0.005f, 20));
    }

    public IEnumerator resetSpeed(float timeInSec, float diff, float diff2)
    {
        yield return new WaitForSeconds(timeInSec);
        speedAdjustTimerChange += diff*-1;
        defSpeedChange += diff2 * -1;
    }

    IEnumerator crash()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        System.Random rand = new System.Random();
        randomDir = rand.NextDouble() >= 0.5 ? -1 : 1;
        float randomRotation = -500 * randomDir;
        float invertedI = 0;
        for (float i = gameObject.transform.localScale.x; i >= 0; i -= Time.deltaTime / 2)
        {
            foreach (GameObject obj in smokes)
            {
                var main = obj.GetComponent<ParticleSystem>().main;
                main.startSize = main.startSize.constant * i / gameObject.transform.localScale.x;
            }
            invertedI += Time.deltaTime / 2;
            gameObject.transform.localScale = new Vector3(i, i, i);
            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x + randomRotation * invertedI, gameObject.transform.rotation.y, gameObject.transform.rotation.z + randomRotation * invertedI);
            yield return null;
        }
    }
}