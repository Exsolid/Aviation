using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(0, -1200, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(-25 * Time.deltaTime, 0, 0, Space.Self);
    }
}
