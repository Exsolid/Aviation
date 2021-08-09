using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
public class AviationEventManager
{
    private static AviationEventManager instance;
    public static AviationEventManager Instance { get { if (instance == null) instance = new AviationEventManager(); return instance; } }

    public event Action onBirdKill;
    public event Action onEnemyKill;
    public event Action<int> onItemPickup;
    public event Action<GameObject> onEnemySpawn;

    public event Action onCloudEnter;
    public event Action onCloudExit;
    public event Action onBooster;

    public AviationEventManager()
    {
    }

    public void onCollision(GameObject ori, GameObject collisionObj)
    {
        if (LayerMask.LayerToName(ori.layer).Equals("Player") && collisionObj.tag.Equals("Birb")) BirdKill(collisionObj);
        else if (LayerMask.LayerToName(collisionObj.layer).Equals("Enemy") && collisionObj.gameObject.GetComponent<EnemyBehaviour>() != null && collisionObj.gameObject.GetComponent<EnemyBehaviour>().currentHealth <= 0)
        {
            EnemyKill(collisionObj);
        }
        else if (ori.tag.Equals("Collectable") && collisionObj.tag.Equals("Player")) ItemPickup(ori);
    }

    private void BirdKill(GameObject obj)
    {
        if(onBirdKill != null && obj != null)
        {
            onBirdKill();
        }
    }

    private void EnemyKill(GameObject obj)
    {
        Debug.Log("1");
        if (onEnemyKill != null && obj != null)
        {
            Debug.Log("2");
            onEnemyKill();
        }
    }

    private void ItemPickup(GameObject obj)
    {
        if (onItemPickup != null && obj != null)
        {
            onItemPickup(obj.GetComponent<ItemID>().id);
        }
    }

    public void EnemySpawn(GameObject obj)
    {
        if (onEnemySpawn != null && obj != null)
        {
            onEnemySpawn(obj);
        }
    }

    public void CloudEnter()
    {
        if (onCloudEnter != null)
        {
            onCloudEnter();
        }
    }

    public void CloudExit()
    {
        if (onCloudExit != null)
        {
            onCloudExit();
        }
    }

    public void Booster()
    {
        if (onBooster != null)
        {
            onBooster();
        }
    }
}
