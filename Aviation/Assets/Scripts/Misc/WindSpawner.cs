using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpawner : MonoBehaviour
{
    private Scaler scaler;
    private float maxDisplayHeightAtGameplay;
    private float maxDisplayWidthAtGameplay;
    private float timer;
    [SerializeField] private float interval;
    [SerializeField] private GameObject objectToSpawn;

    // Start is called before the first frame update
    void Start()
    {

        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        maxDisplayWidthAtGameplay = maxDisplayHeightAtGameplay * Camera.main.aspect;
        scaler = gameObject.GetComponent<Scaler>();
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if (timer > interval)
        {
            spawnObject();
            timer = 0;
        }
    }
    private void spawnObject()
    {
        System.Random rand = new System.Random();
        Vector3 pos = new Vector3(Random.Range(- maxDisplayWidthAtGameplay / 2+5, maxDisplayWidthAtGameplay / 2 - scaler.BorderSizeRight)-5, 0, Random.Range(maxDisplayHeightAtGameplay / 2, maxDisplayHeightAtGameplay / -2));
        GameObject obj = GameObject.Instantiate(objectToSpawn, pos, Quaternion.Euler(90, 90, 0));
        StartCoroutine(deleteEffect(8, obj));

        if(pos.x >= 0) pos = new Vector3(pos.x - 10, 0, rand.NextDouble() >= 0.5 ? pos.z -10 : pos.z +10);
        else pos = new Vector3(pos.x + 10, 0, rand.NextDouble() >= 0.5 ? pos.z - 10 : pos.z + 10);
        obj = GameObject.Instantiate(objectToSpawn, pos, Quaternion.Euler(90, 90, 0));
        StartCoroutine(deleteEffect(8, obj));
    }

    IEnumerator deleteEffect(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
}
