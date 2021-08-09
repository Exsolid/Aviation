using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private bool isHidden;
    [SerializeField] private GameObject cloud;
    [SerializeField] private List<GameObject> objectsToSpawn;
    [SerializeField] private float cloudRatePercentage;
    [SerializeField] GameObject player;
    private Scaler scaler;
    private float maxDisplayHeightAtGameplay;
    private float maxDisplayWidthAtGameplay;
    private float timer;
    private float interval;
    private int counter;
    private bool start;

    private List<GameObject> spawned;
    public List<GameObject> Spawned { get { return new List<GameObject>(spawned); } }
    // Start is called before the first frame update

    void Start()
    {
        if (cloud == null && isHidden == true) Debug.LogError(gameObject.name + ": Cant hide object without a cloud prefab!");
        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        maxDisplayWidthAtGameplay = maxDisplayHeightAtGameplay * Camera.main.aspect;
        StartCoroutine(setValues());
        scaler = gameObject.GetComponent<Scaler>();
        AviationEventManager.Instance.onItemPickup += removeFromSpawnedByID;
        spawned = new List<GameObject>();
    }

    IEnumerator setValues()
    {
        yield return new WaitForEndOfFrame();
        interval = (player.GetComponent<PlayerBehaviourScript>().maxFuel * player.GetComponent<PlayerBehaviourScript>().TimeBetweenFuelLoss - 10) / objectsToSpawn.Count;
        timer = interval / 5;
        start = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (start) {
            timer += Time.deltaTime;
            if (timer > interval && counter < objectsToSpawn.Count)
            {
                spawnObject();
                timer = 0;
            }
        }
    }
    private void spawnObject()
    {
        Vector3 pos = new Vector3(Random.Range(- maxDisplayWidthAtGameplay / 2, maxDisplayWidthAtGameplay / 2 - scaler.BorderSizeRight), 0, maxDisplayHeightAtGameplay/2 +4);
        GameObject obj = GameObject.Instantiate(objectsToSpawn[counter], pos, Quaternion.Euler(0, 0, 0));
        StaticObjectBehaviour movement = obj.GetComponent<StaticObjectBehaviour>();
        movement.Speed = Mathf.Abs(movement.Speed) * -1;
        Scaler scl = obj.AddComponent<Scaler>();
        scl.RightGui = scaler.RightGui;
        scl.Canvas = scaler.Canvas;
        spawned.Add(objectsToSpawn[counter]);
        if (cloud != null && isHidden == true && Random.Range(0, 100) <= cloudRatePercentage)
        {
            GameObject objCloud = GameObject.Instantiate(cloud , pos+Vector3.up*2, Quaternion.Euler(0, 90, 0));
            movement = objCloud.GetComponent<StaticObjectBehaviour>();
            movement.Speed = Mathf.Abs(movement.Speed) * -1;
            scl = objCloud.AddComponent<Scaler>();
            scl.RightGui = scaler.RightGui;
            scl.Canvas = scaler.Canvas;
        }
        counter++;
    }

    private void removeFromSpawnedByID(int id)
    {
        GameObject toRemove = null;
        foreach(GameObject obj in spawned)
        {
            if(obj.GetComponent<ItemID>().id == id)
            {
                toRemove = obj;
                break;
            }
        }
        spawned.Remove(toRemove);
    }
}
