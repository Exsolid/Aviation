using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour
{
    readonly float inputFactor = 20;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        /* Eingabe speichern */
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        float yInput = Input.GetAxis("Height");

        /*Neue Position bestimmen */
        float xNew = transform.position.x + xInput * inputFactor * Time.deltaTime;
        if (xNew < -20f) xNew = -20f;
        else if (xNew > 20f) xNew = 20f;
        float zNew = transform.position.z + zInput * inputFactor * Time.deltaTime;
        if (zNew < -10f) zNew = -10f;
        else if (zNew > 10f) zNew = 10f;
        float yNew = transform.position.y + yInput * inputFactor * Time.deltaTime;
        if (yNew < 1f) yNew = 1f;
        else if (yNew > 10f) yNew = 10f; 
        /* (WIP) float yNew = 1;
        transform.position = new Vector3(xNew, yNew, zNew);
        if (Input.GetButtonDown("Rise (test)"))
        {
            yNew = transform.position.y + 9 * Time.deltaTime;
            transform.position = new Vector3(xNew, yNew, zNew);
        }*/
        transform.position = new Vector3(xNew, yNew, zNew);

    }
}
