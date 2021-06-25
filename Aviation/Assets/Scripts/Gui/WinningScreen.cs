using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningScreen : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("Spitfire-Level");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuMain");
    }
}
