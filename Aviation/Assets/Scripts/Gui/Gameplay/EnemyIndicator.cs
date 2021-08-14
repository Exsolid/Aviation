using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject enemy;
    private float maxDisplayHeightAtGameplay;
    private float timer;
    [SerializeField]private float blinkInterval;
    void Start()
    {
        AviationEventManagerGui.Instance.onEnemySpawn += setIndicate;

        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy != null)
        {
            timer += Time.deltaTime;
            gameObject.transform.position = new Vector3(enemy.transform.position.x, gameObject.transform.position.y, -maxDisplayHeightAtGameplay/2);
            if(timer > blinkInterval)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
                timer = 0;
            }
            if (enemy.transform.position.z > -maxDisplayHeightAtGameplay / 2)
            {
                enemy = null;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    private void setIndicate(GameObject obj)
    {
        enemy = obj;
    }
}
