using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarRatingSave : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject uiBirbKill;
    [SerializeField] private GameObject uiEnemyKill;
    [SerializeField] private float enemyKillScaling;
    [SerializeField] private float birbKillScaling;

    // Start is called before the first frame update
    void Start()
    {
        AviationEventManager.Instance.onWin += saveStars;
    }

    public void saveStars()
    {
        PlayerBehaviourScript pbs = player.GetComponent<PlayerBehaviourScript>();

        float healthPct = pbs.currentHealth/(pbs.maxHealth * 0.5f);
        float extra = (birbKillScaling * uiBirbKill.GetComponent<BirdKillCounter>().Count + enemyKillScaling * uiEnemyKill.GetComponent<EnemyKillCounter>().Count)/ (pbs.maxHealth * 1f);
        if(!PlayerPrefs.HasKey("Aviation_SpitfireStars") || PlayerPrefs.GetInt("Aviation_SpitfireStars") < (int)Math.Ceiling(extra + healthPct)) PlayerPrefs.SetInt("Aviation_SpitfireStars",(int) Math.Ceiling(extra+healthPct));
        PlayerPrefs.SetInt("Aviation_SpitfireStarsCurrent", (int)Math.Ceiling(extra + healthPct));
    }
}
