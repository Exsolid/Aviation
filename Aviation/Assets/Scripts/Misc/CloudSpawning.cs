using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawning : MonoBehaviour
{
    [SerializeField] private GameObject cloud;
    [SerializeField] private GameObject booster;
    [SerializeField] private float spawnRatePercentage;
    [SerializeField] private float interval;
    private List<GameObject> gameObjects;
    private CollectableSpawner collSpawner;
    private Scaler scaler;
    private float maxDisplayHeightAtGameplay;
    private float maxDisplayWidthAtGameplay;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        maxDisplayWidthAtGameplay = maxDisplayHeightAtGameplay * Camera.main.aspect;
        Mathf.Clamp(spawnRatePercentage, 0, 100);
        scaler = gameObject.GetComponent<Scaler>();
        collSpawner = gameObject.GetComponent<CollectableSpawner>();
        gameObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        int per = Random.Range(0, 100);
        if (timer > interval && per >= spawnRatePercentage)
        {
            gameObjects.AddRange(collSpawner.Spawned);
            GameObject toSpawn = null;
            int itemPer = Random.Range(0, 100);
            if (itemPer >= 0 && itemPer < 30) toSpawn = booster;
            else if(gameObjects.Count > 0 && itemPer >= 30 && itemPer < 70) toSpawn = gameObjects[Random.Range(0, gameObjects.Count - 1)];
            spawnObject(toSpawn);
            timer = 0;
            gameObjects.Clear();
        }
    }

    private void spawnObject(GameObject objectToSpawn)
    {
        Vector3 pos = new Vector3(Random.Range(-maxDisplayWidthAtGameplay / 2, maxDisplayWidthAtGameplay / 2 - scaler.BorderSizeRight), 0, maxDisplayHeightAtGameplay/2);
        if (objectToSpawn != null)
        {
            GameObject obj = GameObject.Instantiate(objectToSpawn, pos, Quaternion.Euler(0, 0, 0));
            StaticObjectBehaviour movement = obj.GetComponent<StaticObjectBehaviour>();
            movement.Speed = Mathf.Abs(movement.Speed)* -1;
        }
        if (cloud != null)
        {
            GameObject objCloud = GameObject.Instantiate(cloud, pos + Vector3.up *2, Quaternion.Euler(0, Random.Range(0,90), 0));
            StaticObjectBehaviour movement = objCloud.GetComponent<StaticObjectBehaviour>();
            movement.Speed = Mathf.Abs(movement.Speed) * -1;
            Scaler scl = objCloud.AddComponent<Scaler>();
            scl.RightGui = scaler.RightGui;
            scl.Canvas = scaler.Canvas;
        }
    }
}
