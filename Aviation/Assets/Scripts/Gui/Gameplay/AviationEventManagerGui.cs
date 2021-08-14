using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using AudioBuddyTool;

public class AviationEventManagerGui : MonoBehaviour
{
    private static AviationEventManagerGui instance;
    public static AviationEventManagerGui Instance { get { return instance; } }

    public event Action onBirdKill;
    public event Action onEnemyKill;
    public event Action<int> onItemPickup;
    public event Action<GameObject> onEnemySpawn;

    public event Action onCloudEnter;
    public event Action onCloudExit;
    public event Action onBooster;

    public event Action onWin;
    public event Action onGameOver;

    private HashSet<int> killed;
    public AviationEventManagerGui()
    {
        killed = new HashSet<int>();
    }

    public void Awake()
    {
        reset();
        instance = this;
    }

    private void FixedUpdate()
    {
        killed.Clear();
    }

    public void onCollision(GameObject ori, GameObject collisionObj)
    {
        if (LayerMask.LayerToName(ori.layer).Equals("Player") && collisionObj.tag.Equals("Birb")) BirdKill(collisionObj);
        else if (LayerMask.LayerToName(ori.layer).Equals("Enemy") && ori.gameObject.GetComponent<EnemyBehaviour>() != null)
        {
            if (ori.gameObject.GetComponent<EnemyBehaviour>().currentHealth <= 0) EnemyKill(ori);
        }
        else if (ori.tag.Equals("Collectable") && collisionObj.tag.Equals("Player")) ItemPickup(ori);

        killed.Add(ori.GetInstanceID());
    }

    private void BirdKill(GameObject obj)
    {
        if (onBirdKill != null && obj != null)
        {
            onBirdKill();
        }
    }

    private void EnemyKill(GameObject obj)
    {
        if (onEnemyKill != null && obj != null && !killed.Contains(obj.GetInstanceID()))
        {
            killed.Add(obj.GetInstanceID());
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
    public void GameOver()
    {
        if (onGameOver != null)
        {
            onGameOver();
        }
    }
}