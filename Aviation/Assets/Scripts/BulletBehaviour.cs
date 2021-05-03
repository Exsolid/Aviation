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
    private float maxDisplayHeightAtGameplay;
    // Start is called before the first frame update
    void Start()
    {
        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y - transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = new Vector3(0, 0, speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.transform.position.z + maxDisplayHeightAtGameplay < transform.position.z)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
