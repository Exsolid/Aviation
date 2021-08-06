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
    public AviationEventManager()
    {
    }

    public void onCollision(GameObject ori, GameObject collisionObj)
    {
        if (LayerMask.LayerToName(ori.layer).Equals("Player") && collisionObj.tag.Equals("Birb")) BirdKill(collisionObj);
        if (LayerMask.LayerToName(collisionObj.layer).Equals("Enemy") && collisionObj.gameObject.GetComponent<EnemyBehaviour>() != null && collisionObj.gameObject.GetComponent<EnemyBehaviour>().currentHealth <= 0)
        {
            EnemyKill(collisionObj);
        }
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
}
