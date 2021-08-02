using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;

    //Health Bar always faces the camera
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
