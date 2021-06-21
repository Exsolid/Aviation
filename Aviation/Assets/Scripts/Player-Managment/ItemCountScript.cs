using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private void Update()
    {
        if(score >= 4)
        {
            SceneManager.LoadScene("WinningScreen");
        }
    }
}
