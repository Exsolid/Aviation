using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Muss mit Buildindex angepasst werden. Alternativ LoadScene("*Scenename*");
        SceneManager.LoadScene("Spitfire-Level");
    }

    public void GoToSettingsMenu()
    {
        SceneManager.LoadScene("MenuSettings");
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits"); // Placeholder
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MenuMain");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
