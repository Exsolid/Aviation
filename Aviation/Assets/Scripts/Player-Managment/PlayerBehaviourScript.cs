using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviourScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody playerrb;
    [SerializeField] private Transform FirePoint_1;
    [SerializeField] private Transform FirePoint_2;
    [SerializeField] private GameObject PlayerGunPrefab;
    private GameObject LeftGun;
    private GameObject RightGun;
    public HealthBar healthBar;
    public Fuel fuel;
    [SerializeField] private PlayerInput playerInput = null;
    [SerializeField] private CharacterController controller = null;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 20;
    [SerializeField] float playerbulletForce = 20f;
    private bool Rotation;
    public int maxHealth = 20;
    public int currentHealth;
    public float maxFuel = 20;
    public float currentFuel;
    [SerializeField] private float timeBetweenFuelLoss = 3f;
    private float timeForFuelLoss;
    private readonly float lockPos = 0f;
    private Vector3 inputMovement;

    public PlayerInput PlayerInput => playerInput;


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
        var finalMovement = inputMovement;
        finalMovement *= movementSpeed * Time.deltaTime;
        controller.Move(finalMovement);

        // Player can't leave camera view
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        //FuelConsumption takes effect when time has passed
        if (timeForFuelLoss <= Time.time)
        {
            FuelConsumption(1f);
            timeForFuelLoss = Time.time + timeBetweenFuelLoss;
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        var inputValue = ctx.ReadValue<Vector2>();
        inputMovement = new Vector3(inputValue.x, 0f, inputValue.y);

        float rotationOnZ = 2 * Mathf.Pow(movementSpeed, 2) * 360 * -inputValue.x;
        if (Mathf.Abs(rotationOnZ) > 50) rotationOnZ = 50 * -inputValue.x;
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