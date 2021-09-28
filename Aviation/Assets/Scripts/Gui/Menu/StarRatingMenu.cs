using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRatingMenu : MonoBehaviour
{

    public void updateStars(int index)
    {
        switch (index)
        {
            case 0:
                break;
            case 1:
                setSpitFireStars();
                break;
            case 2:
                setOther();
                break;
        }
    }

    private void setSpitFireStars()
    {
        gameObject.SetActive(true);
        int current = 0;
        if (PlayerPrefs.HasKey("Aviation_SpitfireStars")) current = PlayerPrefs.GetInt("Aviation_SpitfireStars");
        gameObject.transform.Find("1").gameObject.SetActive(current >= 1);
        gameObject.transform.Find("2").gameObject.SetActive(current >= 2);
        gameObject.transform.Find("3").gameObject.SetActive(current >= 3);

    }

    private void setOther()
    {
        gameObject.SetActive(true);
        int current = 0;
        if (PlayerPrefs.HasKey("Aviation_ZeppelinStars")) current = PlayerPrefs.GetInt("Aviation_ZeppelinStars");
        gameObject.transform.Find("1").gameObject.SetActive(current >= 1);
        gameObject.transform.Find("2").gameObject.SetActive(current >= 2);
        gameObject.transform.Find("3").gameObject.SetActive(current >= 3);
    }
}
