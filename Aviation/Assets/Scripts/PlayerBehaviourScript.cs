using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour
{
    [SerializeField] private float inputFactor = 20;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody playerrb;
    [SerializeField] private bool Rotation;
    [SerializeField] private Transform FirePoint_1;
    [SerializeField] private Transform FirePoint_2;
    [SerializeField] private GameObject PlayerGunPrefab;
    [SerializeField] float playerbulletForce = 20f;
    private GameObject LeftGun;
    private GameObject RightGun;

    // Start is called before the first frame update
    void Start()
    {
        playerrb = GetComponent<Rigidbody>();
        playerrb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Input is saved
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        // New position is calculated
        float xNew = transform.position.x + xInput * inputFactor * Time.deltaTime;
        float zNew = transform.position.z + zInput * inputFactor * Time.deltaTime;
        transform.position = new Vector3(xNew, 1, zNew);
        
        // Player can't leave camera view
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        // Player rotates when moving on the x axis
        float rotationOnZ = 2 * Mathf.Pow(inputFactor, 2) * 360 * -xInput;
        if (Mathf.Abs(rotationOnZ) > 50) rotationOnZ = 50 * -xInput;
        if (Rotation) transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotationOnZ);

        // Player can shoot
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        void Shoot()
        {
            LeftGun = Instantiate(PlayerGunPrefab, FirePoint_1.position, FirePoint_1.rotation);
            RightGun = Instantiate(PlayerGunPrefab, FirePoint_2.position, FirePoint_2.rotation);
            Rigidbody bulletrb = LeftGun.GetComponent<Rigidbody>();
            Rigidbody bullet2rb = RightGun.GetComponent<Rigidbody>();
            bulletrb.AddForce(FirePoint_1.forward * playerbulletForce, ForceMode.Impulse);
            bullet2rb.AddForce(FirePoint_2.forward * playerbulletForce, ForceMode.Impulse);
        }
    }
}