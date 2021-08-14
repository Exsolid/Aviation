using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using AudioBuddyTool;

public class AviationEventManagerGui
{
    private static AviationEventManagerGui instance;
    public static AviationEventManagerGui Instance { get { if (instance == null) instance = new AviationEventManagerGui(); return instance; } }

    public event Action onBirdKill;
    public event Action onEnemyKill;
    public event Action<int> onItemPickup;
    public event Action<GameObject> onEnemySpawn;

    public event Action onCloudEnter;
    public event Action onCloudExit;
    public event Action onBooster;

    public event Action onWin;

    public AviationEventManagerGui()
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
        if (onEnemyKill != null && obj != null)
        {
            onEnemyKill();
        }
    }

    private void ItemPickup(GameObject obj)
    {
        if (onItemPickup != null && obj != null)
        {
            AudioBuddy.Play("einsammeln_5", Options.Instance.EffectVolume);
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
            AudioBuddy.Play("einsammeln_5", Options.Instance.EffectVolume);
            onBooster();
        }
    }

    public void reset()
    {
        onBirdKill = null;
        onEnemyKill = null;
        onItemPickup = null;
        onEnemySpawn = null;

        onCloudEnter = null;
        onCloudExit = null;
        onBooster = null;
        onWin = null;
    }

    public void Win()
    {
        if (onWin != null)
        {
            onWin();
        }
    }
}
