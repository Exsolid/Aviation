using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 20;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody playerrb;
    [SerializeField] private Transform FirePoint_1;
    [SerializeField] private Transform FirePoint_2;
    [SerializeField] private GameObject PlayerGunPrefab;
    [SerializeField] float playerbulletForce = 20f;
    private bool Rotation;
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
    private readonly float lockPos = 0f;

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
        Move();
        ZRotation();

        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        
        // Player can't leave camera view
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

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
    }

    public void Move()
    {
        //takes Input Keys from Input System
        var movementInput = controls.Player.Movement.ReadValue<Vector2>();

        //takes the Vector2 Input System and creates a Vector3 which takes the X-Axis Input to control movement on X-Axis
        //and takes Y-Axis Input to control movement on the Z-Axis
        var movement = new Vector3
        {
            x = movementInput.x,
            z = movementInput.y
        }.normalized;
        
        transform.Translate(movementSpeed * Time.deltaTime * movement);
        
    }

    public void ZRotation()
    {
        //takes the Input Keys for "ZRotation" from the Input System
        var ZRotationInput = controls.Player.ZRotation.ReadValue<float>();

        float rotationOnZ = 2 * Mathf.Pow(movementSpeed, 2) * 360 * -ZRotationInput;
        if (Mathf.Abs(rotationOnZ) > 50) rotationOnZ = 50 * -ZRotationInput;
        if (Rotation) transform.rotation = Quaternion.Euler(lockPos, lockPos, rotationOnZ);

    }

    public void Shoot()
    {         
        //Instantiates Bullets at the Gunpoints set on the playerasset
        LeftGun = Instantiate(PlayerGunPrefab, FirePoint_1.position, FirePoint_1.rotation);
        RightGun = Instantiate(PlayerGunPrefab, FirePoint_2.position, FirePoint_2.rotation);
        //adds Rigidbody to the bullets
        Rigidbody bulletrb = LeftGun.GetComponent<Rigidbody>();
        Rigidbody bullet2rb = RightGun.GetComponent<Rigidbody>();
        //adds Force to the bullets which pushes them forward
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