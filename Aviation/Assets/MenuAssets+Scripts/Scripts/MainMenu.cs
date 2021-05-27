using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToSettingsMenu()
    {
        SceneManager.LoadScene("MenuSettings");
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
