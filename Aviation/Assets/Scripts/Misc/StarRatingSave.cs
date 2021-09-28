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
        AviationEventManagerGui.Instance.onWin += saveStars;
    }

    public void saveStars()
    {
        PlayerBehaviourScript pbs = player.GetComponent<PlayerBehaviourScript>();

        float healthPct = pbs.currentHealth/(pbs.maxHealth * 0.5f);
        float extra = AviationEventManagerGui.CurrentLevel == 1
            ? (birbKillScaling * uiBirbKill.GetComponent<BirdKillCounter>().Count + enemyKillScaling * uiEnemyKill.GetComponent<EnemyKillCounter>().Count)/ (pbs.maxHealth * 1f)
            : (birbKillScaling * uiBirbKill.GetComponent<BirdKillCounter>().Count) / (pbs.maxHealth * 1f);
        if (AviationEventManagerGui.CurrentLevel == 1 && (!PlayerPrefs.HasKey("Aviation_SpitfireStars") || PlayerPrefs.GetInt("Aviation_SpitfireStars") < (int)Math.Ceiling(extra + healthPct))) PlayerPrefs.SetInt("Aviation_SpitfireStars",(int) Math.Ceiling(extra+healthPct));
        if (AviationEventManagerGui.CurrentLevel == 2 && (!PlayerPrefs.HasKey("Aviation_ZeppelinStars") || PlayerPrefs.GetInt("Aviation_ZeppelinStars") < (int)Math.Ceiling(extra + healthPct))) PlayerPrefs.SetInt("Aviation_ZeppelinStars", (int)Math.Ceiling(extra + healthPct));
        if (AviationEventManagerGui.CurrentLevel == 1) PlayerPrefs.SetInt("Aviation_SpitfireStarsCurrent", (int)Math.Ceiling(extra + healthPct));
        if (AviationEventManagerGui.CurrentLevel == 2) PlayerPrefs.SetInt("Aviation_ZeppelinStarsCurrent", (int)Math.Ceiling(extra + healthPct));
    }
}
