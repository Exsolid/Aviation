using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 20;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody playerrb;
    [SerializeField] private bool Rotation;
    [SerializeField] private Transform FirePoint_1;
    [SerializeField] private Transform FirePoint_2;
    [SerializeField] private GameObject PlayerGunPrefab;
    [SerializeField] float playerbulletForce = 20f;
    private GameObject LeftGun;
    private GameObject RightGun;
    public int maxHealth = 20;
    public int currentHealth;
    public HealthBar healthBar;
    public float maxFuel = 20;
    public float currentFuel;
    public Fuel fuel;
    [SerializeField] private float timeBetweenFuelLoss = 3f;
    private float timeForFuelLoss;
    private float lockPos = 0;

    private Controls controls = null;
    private void Awake() => controls = new Controls();
    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();

    // Start is called before the first frame update
    void Start()
    {
        playerrb = GetComponent<Rigidbody>();
        playerrb.useGravity = false;
        playerrb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentFuel = maxFuel;
        fuel.SetMaxFuel(maxFuel);
        timeForFuelLoss = Time.time + timeBetweenFuelLoss;
        Rotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Input is saved
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        // New position is calculated
        float xNew = transform.position.x + xInput * movementSpeed * Time.deltaTime;
        float zNew = transform.position.z + zInput * movementSpeed * Time.deltaTime;
        transform.position = new Vector3(xNew, transform.position.y, zNew);
        
        // Player can't leave camera view
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        // Player rotates on Z axis when moving on the X axis
        float rotationOnZ = 2 * Mathf.Pow(movementSpeed, 2) * 360 * -xInput;
        if (Mathf.Abs(rotationOnZ) > 50) rotationOnZ = 50 * -xInput;
        if (Rotation) transform.rotation = Quaternion.Euler(lockPos, lockPos, rotationOnZ);

        // Player can shoot
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        //FuelConsumption takes effect when time has passed
        if (timeForFuelLoss <= Time.time)
        {
            FuelConsumption(1f);
            timeForFuelLoss = Time.time + timeBetweenFuelLoss;
        }

        //Player descends when HP or Fuel reaches 0
        if (currentFuel <= 0 || currentHealth <= 0)
        {
            float yNew = transform.position.y - 10 * Time.deltaTime;
            transform.position = new Vector3(xNew, yNew, zNew);
            Rotation = false;
            transform.Rotate(20 * Time.deltaTime, 0, 50 * Time.deltaTime, Space.Self);
        }
    }

    void Move()
    {
        var movementInput = controls.Player.Movement.ReadValue<Vector2>();

        var movement = new Vector3
        {
            x = movementInput.x,
            z = movementInput.y
        };
        movement.Normalize();
        
        transform.Translate(movementSpeed * Time.deltaTime * movement);
        
    }

    public void Shoot()
    {
        LeftGun = Instantiate(PlayerGunPrefab, FirePoint_1.position, FirePoint_1.rotation);
        RightGun = Instantiate(PlayerGunPrefab, FirePoint_2.position, FirePoint_2.rotation);
        Rigidbody bulletrb = LeftGun.GetComponent<Rigidbody>();
        Rigidbody bullet2rb = RightGun.GetComponent<Rigidbody>();
        bulletrb.AddForce(FirePoint_1.forward * playerbulletForce, ForceMode.Impulse);
        bullet2rb.AddForce(FirePoint_2.forward * playerbulletForce, ForceMode.Impulse);
    }

    void FuelConsumption(float loss)
    {
        currentFuel -= loss;
        fuel.SetFuel(currentFuel);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    void OnCollisionEnter(Collision collision)
    {
        TakeDamage(2);
    }
}