using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private GameObject player;
    [SerializeField] private float interval;
    [SerializeField] private float speed;
    private Scaler scaler;
    private float maxDisplayHeightAtGameplay;
    private float maxDisplayWidthAtGameplay;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        speed = Mathf.Abs(speed) * -1;
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
            spawnEnemy();
            timer = 0;
        }
    }

    private void spawnEnemy()
    {
        GameObject obj = GameObject.Instantiate(enemyToSpawn, new Vector3(Random.Range(- maxDisplayWidthAtGameplay / 2, maxDisplayWidthAtGameplay / 2 - scaler.BorderSizeRight), 0, -maxDisplayHeightAtGameplay), Quaternion.Euler(0, 0, 0));
        EnemyBehaviour enBe = obj.GetComponent<EnemyBehaviour>();
        Scaler scl = obj.AddComponent<Scaler>();
        scl.RightGui = scaler.RightGui;
        scl.Canvas = scaler.Canvas;
        enBe.Player = player;
    }
}
