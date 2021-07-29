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
        Vector3 pos = new Vector3(Random.Range(scaler.BorderSizeLeft - maxDisplayWidthAtGameplay / 2, maxDisplayWidthAtGameplay / 2 - scaler.BorderSizeRight), 0, Random.Range(maxDisplayHeightAtGameplay / 2, maxDisplayHeightAtGameplay / -2));
        GameObject obj = GameObject.Instantiate(objectToSpawn, pos, Quaternion.Euler(90, 90, 0));
        ParticleSystem sys =obj.GetComponent<ParticleSystem>();
        StartCoroutine(deleteEffect(8, obj));
    }

    IEnumerator deleteEffect(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
}
