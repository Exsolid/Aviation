using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    [SerializeField] private float speed;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = new Vector3(0, 0, speed);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
