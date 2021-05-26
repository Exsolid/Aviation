using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private bool isHidden;
    [SerializeField] private bool fromBehind;
    [SerializeField] private GameObject cloud;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private float interval;
    private float maxDisplayHeightAtGameplay;
    private float maxDisplayWidthAtGameplay;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        if (cloud == null && isHidden == true) Debug.LogError(gameObject.name+": Cant hide object without a cloud prefab!");
        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        maxDisplayWidthAtGameplay = maxDisplayHeightAtGameplay * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > interval)
        {
            spawnObject();
            timer = 0;
        }
    }
    private void spawnObject()
    {
        GameObject obj = GameObject.Instantiate(objectToSpawn, new Vector3(Random.Range(maxDisplayWidthAtGameplay / -2, maxDisplayWidthAtGameplay / 2), 0, maxDisplayHeightAtGameplay * (fromBehind ? -1 : 1)), Quaternion.Euler(0, 0, 0));
        StaticObjectBehaviour movement = obj.GetComponent<StaticObjectBehaviour>();
        movement.Speed = Mathf.Abs(movement.Speed) * (fromBehind ? 1 : -1); ;
        if (cloud == null && isHidden == true)
        {
            GameObject objCloud = GameObject.Instantiate(objectToSpawn, new Vector3(Random.Range(maxDisplayWidthAtGameplay / -2, maxDisplayWidthAtGameplay / 2), 10, maxDisplayHeightAtGameplay * (fromBehind ? -1 : 1)), Quaternion.Euler(0, 0, 0));
            movement = objCloud.GetComponent<StaticObjectBehaviour>();
            movement.Speed = Mathf.Abs(movement.Speed) * (fromBehind ? 1 : -1);
        }
    }
}
