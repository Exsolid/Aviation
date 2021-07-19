using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private bool isHidden;
    [SerializeField] private bool fromBehind;
    [SerializeField] private GameObject cloud;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private float spawnRatePercentage;
    [SerializeField] private float cloudRatePercentage;
    [SerializeField] private float interval;
    [SerializeField] private bool triangleSpawn;
    [SerializeField] private bool invertTriangleSpawn;
    private Scaler scaler;
    private float maxDisplayHeightAtGameplay;
    private float maxDisplayWidthAtGameplay;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        if (cloud == null && isHidden == true) Debug.LogError(gameObject.name+": Cant hide object without a cloud prefab!");
        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        maxDisplayWidthAtGameplay = maxDisplayHeightAtGameplay * Camera.main.aspect;
        Mathf.Clamp(spawnRatePercentage, 0, 100);
        scaler = gameObject.GetComponent<Scaler>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        int per = Random.Range(0, 100);
        if (timer > interval && per <= spawnRatePercentage)
        {
            spawnObject();
            timer = 0;
        }
    }
    private void spawnObject()
    {
        Vector3 pos = new Vector3(Random.Range(scaler.BorderSizeLeft - maxDisplayWidthAtGameplay / 2, maxDisplayWidthAtGameplay / 2 - scaler.BorderSizeRight), 0, maxDisplayHeightAtGameplay * (fromBehind ? -1 : 1));
        GameObject obj = GameObject.Instantiate(objectToSpawn, pos, Quaternion.Euler(0, 0, 0));
        StaticObjectBehaviour movement = obj.GetComponent<StaticObjectBehaviour>();
        if(fromBehind) obj.transform.Rotate(new Vector3(0,180,0),Space.Self);
        movement.Speed = Mathf.Abs(movement.Speed) * (fromBehind ? 1 : -1);
        if (triangleSpawn && !isHidden)
        {
            Bounds size = obj.GetComponent<Collider>().bounds;

            obj = GameObject.Instantiate(objectToSpawn, pos + Vector3.right * size.size.x - Vector3.forward * size.size.x * (invertTriangleSpawn ? -1 : 1), Quaternion.Euler(0, 0, 0));
            movement = obj.GetComponent<StaticObjectBehaviour>();
            if (fromBehind) obj.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
            movement.Speed = Mathf.Abs(movement.Speed) * (fromBehind ? 1 : -1);
            Scaler scl = obj.AddComponent<Scaler>();
            scl.LeftGui = scaler.LeftGui;
            scl.RightGui = scaler.RightGui;
            scl.Canvas = scaler.Canvas;

            obj = GameObject.Instantiate(objectToSpawn, pos - Vector3.right * size.size.x - Vector3.forward * size.size.x * (invertTriangleSpawn ? -1 : 1), Quaternion.Euler(0, 0, 0));
            movement = obj.GetComponent<StaticObjectBehaviour>();
            if (fromBehind) obj.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
            movement.Speed = Mathf.Abs(movement.Speed) * (fromBehind ? 1 : -1);
            scl = obj.AddComponent<Scaler>();
            scl.LeftGui = scaler.LeftGui;
            scl.RightGui = scaler.RightGui;
            scl.Canvas = scaler.Canvas;
        }
        if (cloud != null && isHidden == true && Random.Range(0, 100) <= cloudRatePercentage)
        {
            GameObject objCloud = GameObject.Instantiate(cloud , pos + Vector3.up * 2, Quaternion.Euler(0, 90, 0));
            movement = objCloud.GetComponent<StaticObjectBehaviour>();
            movement.Speed = Mathf.Abs(movement.Speed) * (fromBehind ? 1 : -1);
            Scaler scl = objCloud.AddComponent<Scaler>();
            scl.LeftGui = scaler.LeftGui;
            scl.RightGui = scaler.RightGui;
            scl.Canvas = scaler.Canvas;
        }
    }
}
