
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject Player
    {
        set { player = value; }
    }

    void Start()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        speed = Mathf.Abs(speed);
        //Get height and width of the gameplay area (camera view bounds at depth)
        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        maxDisplayWidthAtGameplay = maxDisplayHeightAtGameplay * Camera.main.aspect;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        hitsToDodge = new Collider[5];
        dodgeBoxSize = 16;
    }

    void FixedUpdate()
    {
        xDistanceToPlayer = player.transform.position.x - transform.position.x;
        zDistanceToPlayer = player.transform.position.z - transform.position.z;
        //Creates a raycast (detecting line) that fills 'hit' if it collides with a collider
        Physics.Raycast(gunPosRight.position, Vector3.forward, out hitRight, maxDisplayHeightAtGameplay);
        Physics.Raycast(gunPosLeft.position, Vector3.forward, out hitLeft, maxDisplayHeightAtGameplay);
        countedHits = Physics.OverlapBoxNonAlloc(transform.position, new Vector3(dodgeBoxSize/2, 0, dodgeBoxSize/2), hitsToDodge);
        shootTimer += Time.deltaTime;
        shoot();
        moveBasedOnDistance(xDistanceToPlayer, zDistanceToPlayer, zDistanceToPlayer < 0);
    }
    void OnDrawGizmos()
    {
        Bounds bounds = new Bounds();
        bounds.center = transform.position;
        bounds.size = new Vector3(dodgeBoxSize, 0, dodgeBoxSize);
        //DebugExtension.DrawBounds(bounds, Color.red);
    }
        private void shoot()
    {
        if ((hitRight.transform != null || hitLeft.transform != null) && shootTimer > shootTiming)
        {
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
        float clostestOnX = 0;
        Vector3 clostestOnZ = new Vector3(0,0,0);
        bool disableRotation = false;
        for (int i = 0; i < countedHits; i++)
        {
            if (hitsToDodge[i].tag != "Player" && !((GameObject)hitsToDodge[i].gameObject).Equals(gameObject))
            {
                if (Mathf.Abs(clostestOnX) < Mathf.Abs(hitsToDodge[i].transform.position.x)) clostestOnX = hitsToDodge[i].transform.position.x;
                if (Mathf.Abs(clostestOnZ.z) < Mathf.Abs(hitsToDodge[i].transform.position.z) && hitsToDodge[i].transform.position.z > transform.position.z) clostestOnZ = hitsToDodge[i].transform.position;
            }
        }
        float speedOnX = 0;
        float speedOnZ = 0;
        //               speed * (function that returns -1 to 1 based on the distance to the player                                    * invers direction          + default speed) * framerate edit 
        if (clostestOnX == 0) speedOnX = speed * (Mathf.Abs(Mathf.Pow(((distanceX) / maxDisplayWidthAtGameplay), 2)) + 0.1f) * Time.deltaTime * getDirectionOnX();
        else
        {
            speedOnX = speed * (Mathf.Abs(Mathf.Pow(((clostestOnX - transform.position.x) / maxDisplayWidthAtGameplay), 2)) + 0.1f) * Time.deltaTime * (clostestOnX > transform.position.x ? -1 : 1);
            disableRotation = true;
        }
            //               speed * function that returns 0-1 based on the distance to the player                                  * direction to move * framerate edit
        if (clostestOnZ.z == 0) speedOnZ = speed * (Mathf.Abs(Mathf.Pow((distanceZ - minDistanceToPlayer) / maxDisplayHeightAtGameplay, 2) + (inversZ ? -1 : 0)) + (inversZ ? 0.05f : -0.05f)) * Time.deltaTime;
        else
        {
            speedOnZ = speed * (Mathf.Abs(Mathf.Pow((clostestOnZ.z - transform.position.z - minDistanceToPlayer) / maxDisplayHeightAtGameplay, 2)) + (zDistanceToPlayer < 0 ? 0.05f : -0.05f)) * Time.deltaTime;
            speedOnX = speed * (Mathf.Abs(Mathf.Pow(((clostestOnZ.x - transform.position.x) / maxDisplayWidthAtGameplay), 2)) + 0.3f) * Time.deltaTime * (clostestOnZ.x > transform.position.x ? -1 : 1);
            disableRotation = true;
        }
        //Cap speed because zDistanceToPlayer (zDistanceToPlayer - minDistanceToPlayer) / maxDisplayHeightAtGameplay can return big numbers if player to enemy distance gets big
        if (speedOnZ > speed / 50) speedOnZ = speed / 50;
        rb.velocity = new Vector3(smooth(rb.velocity.x, speedOnX), 0, smooth(rb.velocity.z, speedOnZ));

                        //function that returns 0-1 based on the distance to the player 
        float rotationOnZ = 3 * Mathf.Pow((distanceX / maxDisplayWidthAtGameplay), 2) * 360 * (clostestOnX == 0 ? -getDirectionOnX() : (clostestOnX < transform.position.x ? -1 : 1));
        if (Mathf.Abs(rotationOnZ) > 45) rotationOnZ = 45 * (clostestOnX == 0 ? -getDirectionOnX() : (clostestOnX < transform.position.x ? -1 : 1));
        if (enableRotationOnMove && !disableRotation) transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, smooth(transform.rotation.eulerAngles.z - 360, rotationOnZ));
    }

    private float smooth(float current, float toSet)
    {
        if (toSet == 0) return current;
        current = toSet*(Mathf.Abs(toSet) / (Mathf.Abs(current) + Mathf.Abs(toSet)));
        return current;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}