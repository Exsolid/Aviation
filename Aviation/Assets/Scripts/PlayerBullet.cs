using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // Destroys bullet when it collides with another object
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    // Destroys bullet when it leaves camera view
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
