using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
public class AviationEventManager
{
    private static AviationEventManager instance;
    public static AviationEventManager Instance { get { if (instance == null) instance = new AviationEventManager(); return instance; } }

    public event Action onBirdKill;

    private List<int> countedBirbIDs;

    public AviationEventManager()
    {
        countedBirbIDs = new List<int>();
    }

    public void onCollision(GameObject ori, GameObject collisionObj)
    {
        if (ori.tag.Equals("Player") && collisionObj.tag.Equals("Birb")) BirdKill(collisionObj);
    }

    private void BirdKill(GameObject obj)
    {
        if(onBirdKill != null && obj != null)
        {
            onBirdKill();
        }
    }
}
