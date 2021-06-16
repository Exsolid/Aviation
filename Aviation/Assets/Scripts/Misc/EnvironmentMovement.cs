using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<GameObject> envParts;
    private List<GameObject> spawnedParts;
    private GameObject toDestroy;
    [SerializeField] private float speed;
    private int counter;
    private Camera cam;
    private float maxDisplayHeightAtGameplay;

    void Start()
    {
        if (gameObject.GetComponent<Camera>() == null) Debug.LogError("The attachted gameObject " + gameObject.name + " is not a camera!");
        else{
            cam = gameObject.GetComponent<Camera>();
            cam.orthographic = true;
            //Set the size of the camera to the width of the background
            cam.orthographicSize = envParts[0].GetComponent<Renderer>().bounds.size.x / cam.aspect / 2 - 1;
            //Get size values of the camera
            maxDisplayHeightAtGameplay = cam.orthographicSize * 2;
            speed = -Mathf.Abs(speed);
            spawnedParts = new List<GameObject>();
            if (envParts != null)
            {
                spawnTile();
                spawnTile();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cam != null && envParts != null)
        {
            spawnedParts.ForEach(delegate (GameObject obj)
            {
                if (obj.GetComponent<Renderer>().bounds.size.z / 2 + obj.transform.position.z < cam.transform.position.z - maxDisplayHeightAtGameplay / 2)
                {
                    toDestroy = obj;
                }
            });
            if(toDestroy != null)
            {
                spawnedParts.Remove(toDestroy);
                Destroy(toDestroy);
                spawnTile();
            }
        }
    }

    public void spawnTiles()
    {
        envParts.ForEach(delegate (GameObject obj)
        {
            GameObject spawned;
            if (spawnedParts.Count == 0) spawned = GameObject.Instantiate(obj, new Vector3(cam.transform.position.x, 0, cam.transform.position.z - 5), Quaternion.Euler(0, 0, 0));
            else spawned = GameObject.Instantiate(obj, new Vector3(cam.transform.position.x, 0, spawnedParts[spawnedParts.Count-1].transform.position.z), Quaternion.Euler(0, 0, 0));
            spawned.transform.position += new Vector3(0,0, spawned.GetComponent<Renderer>().bounds.size.z * counter);
            Rigidbody rb = spawned.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = new Vector3(0, 0, speed);
            counter++;
            spawnedParts.Add(spawned);
        });
        counter = 0;
    }

    public void spawnTile()
    {
        GameObject spawned;
        if (spawnedParts.Count == 0) spawned = GameObject.Instantiate(envParts[counter], new Vector3(cam.transform.position.x, -5, cam.transform.position.z), Quaternion.Euler(0, 0, 0));
        else
        {
            spawned = GameObject.Instantiate(envParts[counter], new Vector3(cam.transform.position.x, -5, spawnedParts[0].transform.position.z-0.12f), Quaternion.Euler(0, 0, 0));
            spawned.transform.position += new Vector3(0, 0, (spawned.GetComponent<Renderer>().bounds.size.z)* spawnedParts.Count);
        }
        Rigidbody rb = spawned.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = new Vector3(0, 0, speed);
        counter++;
        spawnedParts.Add(spawned);
        if (counter == envParts.Count) counter = 0;
    }
}
