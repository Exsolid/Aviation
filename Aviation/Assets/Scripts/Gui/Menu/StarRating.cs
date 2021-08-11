using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRating : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int stars = 0;
        if(PlayerPrefs.HasKey("Aviation_SpitfireStarsCurrent")) stars = PlayerPrefs.GetInt("Aviation_SpitfireStarsCurrent");
        gameObject.transform.Find("1").gameObject.SetActive(stars >= 1);
        gameObject.transform.Find("2").gameObject.SetActive(stars >= 2);
        gameObject.transform.Find("3").gameObject.SetActive(stars >= 3);
        int current = 0;
        if (PlayerPrefs.HasKey("Aviation_SpitfireStars")) current = PlayerPrefs.GetInt("Aviation_SpitfireStars");
        gameObject.transform.Find("Text").gameObject.SetActive(stars < current);
    }
}
