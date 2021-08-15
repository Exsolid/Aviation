
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioBuddyTool;
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed;
    [SerializeField] private float minDistanceToPlayer;
    [SerializeField] private bool enableRotationOnMove;
    private Rigidbody rb;
    private float maxDisplayHeightAtGameplay;
    private float maxDisplayWidthAtGameplay;
    private float xDistanceToPlayer;
    private float zDistanceToPlayer;

    private Collider[] hitsToDodge;
    private int countedHits;
    private int dodgeBoxSize;

    [SerializeField] private Transform gunPosLeft;
    [SerializeField] private Transform gunPosRight;
    [SerializeField] private float shootTiming;
    [SerializeField] private GameObject bulletPrefab;
    private float shootTimer;
    private RaycastHit hitLeft;
    private RaycastHit hitRight;

    public HealthBar healthBar;
    public int maxHealth;
    public int currentHealth;
    [SerializeField] private GameObject smoke;
    private List<GameObject> smokes;

    private bool crashing;
    private int randomDir;

    [SerializeField] private float durationOfStay;
    private bool flyAway;
    public GameObject Player
    {
        set { player = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    void Start()
    {
        smokes = new List<GameObject>();
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        speed = Mathf.Abs(speed);
        //Get height and width of the gameplay area (camera view bounds at depth)
        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        maxDisplayWidthAtGameplay = maxDisplayHeightAtGameplay * Camera.main.aspect;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        hitsToDodge = new Collider[5];
        dodgeBoxSize = 35;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        StartCoroutine(stayTimer());
    }

    void FixedUpdate()
    {
        if (!crashing)
        {
            xDistanceToPlayer = player.transform.position.x - transform.position.x;
            zDistanceToPlayer = player.transform.position.z - transform.position.z;
            //Creates a raycast (detecting line) that fills 'hit' if it collides with a collider
            Physics.Raycast(gunPosRight.position, Vector3.forward, out hitRight, maxDisplayHeightAtGameplay);
            Physics.Raycast(gunPosLeft.position, Vector3.forward, out hitLeft, maxDisplayHeightAtGameplay);
            countedHits = Physics.OverlapBoxNonAlloc(transform.position, new Vector3(dodgeBoxSize / 2, 0, dodgeBoxSize / 2), hitsToDodge);
            shootTimer += Time.deltaTime;
            shoot();
            moveBasedOnDistance(xDistanceToPlayer, zDistanceToPlayer, zDistanceToPlayer < 0);
        }

        if (currentHealth <= 0 && !crashing)
        {
            crashing = true;
            StartCoroutine(crash());
        }
        if (crashing)
        {
            rb.velocity = new Vector3(speed / 200f * randomDir, 0, rb.velocity.z);
        }
    }

    private IEnumerator stayTimer()
    {
        yield return new WaitForSeconds(durationOfStay);
        flyAway = true;
    }

    private void shoot()
    {
        if ((hitRight.transform != null || hitLeft.transform != null) && shootTimer > shootTiming)
        {
            AudioBuddy.Play("gun_cut", Options.Instance.EffectVolume);
            shootTimer = 0;
            if (gunPosLeft != null && bulletPrefab != null)
            {
                //Create bullet and add the planes speed to the bullet speed
                GameObject bullet = GameObject.Instantiate(bulletPrefab, gunPosLeft.transform.position, Quaternion.Euler(0, 0, 0));
                StaticObjectBehaviour behaviour = bullet.GetComponent<StaticObjectBehaviour>();
                behaviour.Speed += behaviour.Speed + speed * Mathf.Pow((zDistanceToPlayer - minDistanceToPlayer) / maxDisplayHeightAtGameplay, 2) * Time.deltaTime;
            }
            if (gunPosRight != null && bulletPrefab != null)
            {
                //Create bullet and add the planes speed to the bullet speed
                GameObject bullet = GameObject.Instantiate(bulletPrefab, gunPosRight.transform.position, Quaternion.Euler(0, 0, 0));
                StaticObjectBehaviour behaviour = bullet.GetComponent<StaticObjectBehaviour>();
                behaviour.Speed += behaviour.Speed + speed * Mathf.Pow((zDistanceToPlayer - minDistanceToPlayer) / maxDisplayHeightAtGameplay, 2) * Time.deltaTime;
            }
        }
    }

    //Returns 1 if the player is on the right, -1 if on the left and 0 if it is close to centered
    private int getDirectionOnX()
    {
        if (player.transform.position.x - transform.position.x > 0.2) return 1;
        else if (player.transform.position.x - transform.position.x < -0.2) return -1;
        else return 0;
    }

    private void moveBasedOnDistance(float distanceX, float distanceZ, bool inversZ)
    {
        if (flyAway) inversZ = true;
        float clostestOnX = 0;
        Vector3 clostestOnZ = new Vector3(0,0,0);
        bool disableRotation = false;
        for (int i = 0; i < countedHits; i++)
        {
            if ((hitsToDodge[i].tag != "Player" || inversZ) && hitsToDodge[i].tag != "Bullet" && !((GameObject)hitsToDodge[i].gameObject).Equals(gameObject))
            {
                if (Mathf.Abs(clostestOnX) < Mathf.Abs(hitsToDodge[i].transform.position.x)) clostestOnX = hitsToDodge[i].transform.position.x;
                if (Mathf.Abs(clostestOnZ.z) < Mathf.Abs(hitsToDodge[i].transform.position.z) && hitsToDodge[i].transform.position.z > transform.position.z) clostestOnZ = hitsToDodge[i].transform.position;
            }
        }

        float speedOnX = 0;
        float speedOnZ = 0;
        //               speed * (function that returns -1 to 1 based on the distance to the player                                    * invers direction          + default speed) * framerate edit 

        if (clostestOnX == 0 && !inversZ && Mathf.Abs(distanceX) > 1f) speedOnX = speed * (Mathf.Abs(Mathf.Pow(((distanceX - transform.position.x) / maxDisplayWidthAtGameplay), 2)) + 0.2f) * Time.deltaTime * getDirectionOnX();
        else if (clostestOnX != 0)
        {
            speedOnX = speed * (Mathf.Abs(Mathf.Pow(((clostestOnX - transform.position.x) / maxDisplayWidthAtGameplay), 2)) + 0.4f) * Time.deltaTime * (clostestOnX > transform.position.x ? -1 : 1);
            disableRotation = true;
            smooth(rb.velocity.x, speedOnX);
        }
        //               speed * function that returns 0-1 based on the distance to the player                                  * direction to move * framerate edit
        if (clostestOnZ.z == 0) speedOnZ = speed * 1.5f * (Mathf.Abs(Mathf.Pow((inversZ ? speed * 0.013f : distanceZ - minDistanceToPlayer) / maxDisplayHeightAtGameplay, 2)) + (inversZ ? 0.02f : -0.05f)) * Time.deltaTime;
        else
        {
            speedOnZ = speed * (Mathf.Abs(Mathf.Pow((clostestOnZ.z - transform.position.z - minDistanceToPlayer) / maxDisplayHeightAtGameplay, 2)) + ((zDistanceToPlayer < 0) ? 0.05f : -0.05f)) * Time.deltaTime;
            speedOnX = speed * (Mathf.Abs(Mathf.Pow(((clostestOnZ.x - transform.position.x) / maxDisplayWidthAtGameplay), 2)) + 0.3f) * Time.deltaTime * (clostestOnZ.x > transform.position.x ? -1 : 1);
            disableRotation = true;
            smooth(rb.velocity.x, speedOnX);
        }
        //Cap speed because zDistanceToPlayer (zDistanceToPlayer - minDistanceToPlayer) / maxDisplayHeightAtGameplay can return big numbers if player to enemy distance gets big
        if (speedOnZ > speed / 50) speedOnZ = speed / 50;
        rb.velocity = new Vector3(speedOnX, 0, smooth(rb.velocity.z, speedOnZ));

                        //function that returns 0-1 based on the distance to the player 
        float rotationOnZ = speedOnX/ (speed / 100) * -35;
        if (enableRotationOnMove && !disableRotation) transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotationOnZ);
    }

    private float smooth(float current, float toSet)
    {
        if (toSet == 0 || Mathf.Abs(Mathf.Abs(current) - Mathf.Abs(toSet)) < 5f)
        {
            return current;
        }
        current = toSet*(Mathf.Abs(toSet) / (Mathf.Abs(current)*1.4f + Mathf.Abs(toSet)));
        return current;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.reduceHealth(damage);
        GameObject obj = Instantiate(smoke, new Vector3(transform.position.x + Random.Range(-gameObject.GetComponent<Collider>().bounds.size.x / 2.75f, gameObject.GetComponent<Collider>().bounds.size.x / 2.75f), transform.position.y - gameObject.GetComponent<Collider>().bounds.size.y, transform.position.z + gameObject.GetComponent<Collider>().bounds.size.z / 5f), Quaternion.Euler(180, 0, 0));
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

    IEnumerator crash()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        System.Random rand = new System.Random();
        randomDir = rand.NextDouble() >= 0.5 ? -1 : 1;
        float randomRotation = -500* randomDir;
        float invertedI = 0;
        for (float i = gameObject.transform.localScale.x; i >= 0; i -= Time.deltaTime/2)
        {
            foreach (GameObject obj in smokes)
            {
                var main = obj.GetComponent<ParticleSystem>().main;
                main.startSize = main.startSize.constant * i / gameObject.transform.localScale.x;
            }
            invertedI += Time.deltaTime / 2;
            gameObject.transform.localScale = new Vector3(i, i, i);
            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x + randomRotation * invertedI, gameObject.transform.rotation.y, gameObject.transform.rotation.z+ randomRotation * invertedI);
            yield return null;
        }
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}