
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed;
    [SerializeField] private float minDistanceToPlayer;
    [SerializeField] private bool enableRotationOnMove;
    [SerializeField] private Rigidbody rb;
    private float maxDisplayHeightAtGameplay;
    private float maxDisplayWidthAtGameplay;
    private float xDistanceToPlayer;
    private float zDistanceToPlayer;

    [SerializeField] private Transform gunPosOne;
    [SerializeField] private Transform gunPosTwo;
    [SerializeField] private float shootTiming;
    [SerializeField] private GameObject bulletPrefab;
    private float shootTimer;
    private RaycastHit hit;

    void Start()
    {
        speed = Mathf.Abs(speed);
        //Get height and width of the gameplay area (camera view bounds at depth)
        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y - player.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        maxDisplayWidthAtGameplay = maxDisplayHeightAtGameplay * Camera.main.aspect;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if(Camera.main.transform.position.z + maxDisplayHeightAtGameplay > transform.position.z) {
            xDistanceToPlayer = player.transform.position.x - transform.position.x;
            zDistanceToPlayer = player.transform.position.z - transform.position.z;
            //Creates a raycast (detecting line) that fills 'hit' if it collides with a collider
            Physics.Raycast(gunPosOne.position, transform.TransformDirection(Vector3.forward), out hit, maxDisplayHeightAtGameplay);
            shootTimer += Time.deltaTime;
            move();
            shoot();
        }
        else
        {
            //Delete object if out of vision
            GameObject.Destroy(gameObject);
        }
      
    }

    private void move()
    {
        //               speed * function that returns 0-1 based on the distance to the player                                  * direction to move * framerate edit
        float speedOnY = speed * 4f * Mathf.Pow(((xDistanceToPlayer + 15 * getDirectionOnX()) / maxDisplayWidthAtGameplay), 2) * getDirectionOnX() * Time.deltaTime;
        //               speed * function that returns 0-1 based on the distance to the player                                  * direction to move * framerate edit
        float speedOnZ = speed * Mathf.Pow((zDistanceToPlayer - minDistanceToPlayer) / maxDisplayHeightAtGameplay, 2)  * Time.deltaTime;
        //Cap speed because zDistanceToPlayer (zDistanceToPlayer - minDistanceToPlayer) / maxDisplayHeightAtGameplay can return big numbers if player to enemy distance gets big
        if (speedOnZ > speed / 50) speedOnZ = speed / 50;
        rb.velocity = new Vector3(speedOnY, 0, speedOnZ);

        //                function that returns 0-1 based on the distance to the player 
        float rotationOnZ = 2*Mathf.Pow((xDistanceToPlayer / maxDisplayWidthAtGameplay),2) * 360 * -getDirectionOnX();
        if (Mathf.Abs(rotationOnZ) > 45) rotationOnZ = 45 * -getDirectionOnX();
        if (enableRotationOnMove) transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotationOnZ);
    }


    private void shoot()
    {
        if (hit.transform != null && shootTimer > shootTiming)
        {
            shootTimer = 0;
            if (gunPosOne != null && bulletPrefab != null)
            {
                //Create bullet and add the planes speed to the bullet speed
                GameObject bullet = GameObject.Instantiate(bulletPrefab, gunPosOne.transform.position, Quaternion.Euler(0, 0, 0));
                BulletBehaviour behaviour = bullet.GetComponent<BulletBehaviour>();
                behaviour.Speed += behaviour.Speed + speed * Mathf.Pow((zDistanceToPlayer - minDistanceToPlayer) / maxDisplayHeightAtGameplay, 2) * Time.deltaTime;
            }
            if(gunPosTwo != null && bulletPrefab != null)
            {
                //Create bullet and add the planes speed to the bullet speed
                GameObject bullet = GameObject.Instantiate(bulletPrefab, gunPosTwo.transform.position, Quaternion.Euler(0, 0, 0));
                BulletBehaviour behaviour = bullet.GetComponent<BulletBehaviour>();
                behaviour.Speed += speed * Mathf.Pow((zDistanceToPlayer - minDistanceToPlayer) / maxDisplayHeightAtGameplay, 2) * Time.deltaTime;
            }
        }
    }

    //Returns 1 if the player is on the right, -1 if on the left and 0 if it is close to centered
    private int getDirectionOnX()
    {
        if (player.transform.position.x - transform.position.x > 0.1) return 1;
        else if (player.transform.position.x - transform.position.x < -0.1) return -1;
        else return 0;
    }
}