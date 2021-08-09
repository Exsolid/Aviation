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
    private GameObject LeftGun;
    private GameObject RightGun;
    public HealthBar healthBar;
    [SerializeField] private PlayerInput playerInput = null;
    [SerializeField] private CharacterController controller = null;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 20;
    private bool Rotation;
    public int maxHealth;
    public int currentHealth;
    public int fuel;
    [SerializeField] private float timeBetweenFuelLoss = 3f;
    private float timeForFuelLoss;
    private readonly float lockPos = 0f;
    private Transform cameraTransform;
    private float defSpeed;

    public PlayerInput PlayerInput => playerInput;

    public float TimeBetweenFuelLoss { get { return timeBetweenFuelLoss; } }
    private Scaler scaler;
    private float maxDisplayHeightAtGameplay;
    private float maxDisplayWidthAtGameplay;

    [SerializeField]private float shootTiming;
    private float shootTimer;

    // Start is called before the first frame update
    void Start()
    {
        shootTimer = 0;
        playerrb = GetComponent<Rigidbody>();
        playerrb.useGravity = false;
        playerrb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionY;
        currentHealth = maxHealth+ fuel;
        healthBar.SetMaxHealth(fuel+maxHealth);
        timeForFuelLoss = Time.time + timeBetweenFuelLoss;
        Rotation = true;
        cameraTransform = Camera.main.transform;
        defSpeed = movementSpeed;
        scaler = gameObject.GetComponent<Scaler>();
        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        maxDisplayWidthAtGameplay = maxDisplayHeightAtGameplay * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        playerInput.actions["Shoot"].performed += _ => Shoot();
        Vector2 input = playerInput.actions["Movement"].ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right + move.z * cameraTransform.up;
        move.y = 0f;
        controller.enabled = true;
        controller.Move(Time.deltaTime * movementSpeed * move);
        controller.enabled = false;

        float rotationOnZ = 2 * Mathf.Pow(movementSpeed, 2) * 360 * -input.x;
        if (Mathf.Abs(rotationOnZ) > 50) rotationOnZ = 50 * -input.x;
        if (Rotation) transform.rotation = Quaternion.Euler(lockPos, lockPos, rotationOnZ);

        // Player can't leave camera view
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, - maxDisplayWidthAtGameplay / 2, maxDisplayWidthAtGameplay/2 - scaler.BorderSizeRight);
        pos.z = Mathf.Clamp(pos.z, -maxDisplayHeightAtGameplay/2, maxDisplayHeightAtGameplay/2);
        transform.position = pos;

        //FuelConsumption takes effect when time has passed
        if (timeForFuelLoss <= Time.time)
        {
            FuelConsumption(1f);
            timeForFuelLoss = Time.time + timeBetweenFuelLoss;
        }

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOverScreen");
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.tag.Equals("Collectable"))
        {
            AudioBuddy.Play("metal_hit", Options.Instance.EffectVolume);
            TakeDamage(2);
        }
        AviationEventManager.Instance.onCollision(gameObject, collision.gameObject);
    }

    public void reduceSpeed()
    {
        float diff = movementSpeed / 5 * -1;
        movementSpeed += diff;
        StartCoroutine(resetSpeed(5, diff));
    }

    public void increaseSpeed()
    {
        float diff = defSpeed * 2/3;
        movementSpeed += diff;
        StartCoroutine(resetSpeed(5, diff));
    }

    public IEnumerator resetSpeed(float timeInSec, float diff)
    {
        yield return new WaitForSeconds(timeInSec);
        diff *= -1;
        movementSpeed += diff;
    }
}