
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float minDistanceToPlayer;
    private float maxDisplayHeightAtGameplay;
    private float maxDisplayWidthAtGameplay;
    private float xDistanceToPlayer;
    private float zDistanceToPlayer;

    void Start()
    {
        speed = Mathf.Abs(speed)*2;
        float frustumHeight = 2.0f * (Mathf.Abs(Camera.main.transform.position.y - player.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        maxDisplayWidthAtGameplay = frustumHeight * Camera.main.aspect; ;
        maxDisplayHeightAtGameplay = frustumHeight;
    }

    void FixedUpdate()
    {
        if(Camera.main.transform.position.z + maxDisplayHeightAtGameplay > transform.position.z) {
            xDistanceToPlayer = player.transform.position.x - transform.position.x;
            zDistanceToPlayer = player.transform.position.z - transform.position.z;
            if (zDistanceToPlayer > maxDisplayHeightAtGameplay) zDistanceToPlayer = maxDisplayHeightAtGameplay;
            tailPlayer();
        }
        else
        {
            gameObject.SetActive(false);
        }
      
    }

    private void tailPlayer()
    {
        rb.velocity = new Vector3(speed * 2 * Mathf.Pow((xDistanceToPlayer / maxDisplayWidthAtGameplay),2) * getDirectionOnX() * Time.deltaTime, 0, Mathf.Pow((zDistanceToPlayer - minDistanceToPlayer) / maxDisplayHeightAtGameplay, 2) * speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, (xDistanceToPlayer / 50) * (xDistanceToPlayer / 50) * 360 * -getDirectionOnX());
    }


    private void move(Vector2 direction)
    {
        rb.velocity = new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0) * direction;
    }

    private int getDirectionOnX()
    {
        if (player.transform.position.x - transform.position.x > 0.1) return 1;
        else if (player.transform.position.x - transform.position.x < -0.1) return -1;
        else return 0;
    }
}