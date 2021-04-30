using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour
{
    readonly float inputFactor = 20;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody playerrb;
    public float xAngle, yAngle, zAngle;

    // Start is called before the first frame update
    void Start()
    {
        playerrb = GetComponent<Rigidbody>();
        playerrb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Eingabe speichern
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        // Neue Position bestimmen
        float xNew = transform.position.x + xInput * inputFactor * Time.deltaTime;
        float zNew = transform.position.z + zInput * inputFactor * Time.deltaTime;
        transform.position = new Vector3(xNew, 1, zNew);
        
        // Objekt bleibt im Kamerabild
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        // Player dreht sich bei Steuerung
        if(xInput < 0)
        {
            transform.Rotate(0, 0, 50 * Time.deltaTime, Space.Self);
        }
        else if (xInput > 0)
        {
            transform.Rotate(0, 0, -50 * Time.deltaTime, Space.Self);
        }
    }
}
