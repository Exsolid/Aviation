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
        float yInput = Input.GetAxis("TestHeight");

        /*Neue Position bestimmen */
        float xNew = transform.position.x + xInput * inputFactor * Time.deltaTime;
        float zNew = transform.position.z + zInput * inputFactor * Time.deltaTime;
        float yNew = transform.position.y + yInput * inputFactor * Time.deltaTime;
        transform.position = new Vector3(xNew, yNew, zNew);
    }
}
