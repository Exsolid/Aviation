using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRatingWinning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int stars = 0;
        if(PlayerPrefs.HasKey("Aviation_SpitfireStarsCurrent") && AviationEventManagerGui.CurrentLevel == 1) stars = PlayerPrefs.GetInt("Aviation_SpitfireStarsCurrent");
        if (PlayerPrefs.HasKey("Aviation_ZeppelinStarsCurrent") && AviationEventManagerGui.CurrentLevel == 2) stars = PlayerPrefs.GetInt("Aviation_ZeppelinStarsCurrent");
        gameObject.transform.Find("1").gameObject.SetActive(stars >= 1);
        gameObject.transform.Find("2").gameObject.SetActive(stars >= 2);
        gameObject.transform.Find("3").gameObject.SetActive(stars >= 3);
        int current = 0;
        if (PlayerPrefs.HasKey("Aviation_SpitfireStars") && AviationEventManagerGui.CurrentLevel == 1) current = PlayerPrefs.GetInt("Aviation_SpitfireStars");
        if (PlayerPrefs.HasKey("Aviation_ZeppelinStars") && AviationEventManagerGui.CurrentLevel == 2) current = PlayerPrefs.GetInt("Aviation_ZeppelinStars");
        gameObject.transform.Find("Text").gameObject.SetActive(stars < current);
    }
}
