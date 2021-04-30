using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour
{
    readonly float inputFactor = 20;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody playerrb;
    private float maxDisplayHeightAtGameplay;
    private float maxDisplayWidthAtGameplay;
    // Start is called before the first frame update
    void Start()
    {
        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y - player.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        maxDisplayWidthAtGameplay = maxDisplayHeightAtGameplay * Camera.main.aspect;
        playerrb = GetComponent<Rigidbody>();
        playerrb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        /* Eingabe speichern */
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        /*Neue Position bestimmen */
        float xNew = transform.position.x + xInput * inputFactor * Time.deltaTime;
        float zNew = transform.position.z + zInput * inputFactor * Time.deltaTime;
        transform.position = new Vector3(xNew, 1, zNew);

    }
}
