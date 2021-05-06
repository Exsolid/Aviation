using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCountScript : MonoBehaviour
{
    public static ItemCountScript instance;
    public Text text;
    int score;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    //ItemCounter updates the displayed text according to collected item
    public void ChangeCount(int itemValue)
    {
        score += itemValue;
        text.text = "Items : " + score.ToString();
    }
}
