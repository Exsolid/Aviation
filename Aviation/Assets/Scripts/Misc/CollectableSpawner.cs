using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private bool isHidden;
    [SerializeField] private GameObject cloud;
    [SerializeField] private List<GameObject> objectsToSpawn;
    private float maxDisplayHeightAtGameplay;
    private float maxDisplayWidthAtGameplay;
    private float timer;
    private float interval;
    private int counter;
    private bool start;
    // Start is called before the first frame update
    void Start()
    {
        if (cloud == null && isHidden == true) Debug.LogError(gameObject.name + ": Cant hide object without a cloud prefab!");
        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        maxDisplayWidthAtGameplay = maxDisplayHeightAtGameplay * Camera.main.aspect;
        StartCoroutine(setValues());
    }

    IEnumerator setValues()
    {
        yield return new WaitForEndOfFrame();
        interval = (Fuel.CurrentValue * 3 - 10) / objectsToSpawn.Count;
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
        Vector3 pos = new Vector3(Random.Range(maxDisplayWidthAtGameplay / -2, maxDisplayWidthAtGameplay / 2), 0, maxDisplayHeightAtGameplay);
        GameObject obj = GameObject.Instantiate(objectsToSpawn[counter], pos, Quaternion.Euler(0, 0, 0));
        StaticObjectBehaviour movement = obj.GetComponent<StaticObjectBehaviour>();
        movement.Speed = Mathf.Abs(movement.Speed) * -1;
        if (cloud != null && isHidden == true)
        {
            GameObject objCloud = GameObject.Instantiate(cloud , pos+Vector3.up*2, Quaternion.Euler(0, 90, 0));
            movement = objCloud.GetComponent<StaticObjectBehaviour>();
            movement.Speed = Mathf.Abs(movement.Speed) * -1;
        }
        counter++;
    }
}
