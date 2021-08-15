using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuManager  : MonoBehaviour
{
    [SerializeField] private GameObject buttonBar;
    [SerializeField] private GameObject rating;
    [SerializeField] private GameObject startGame;

    private List<Image> uiElements;
    private int index;
    void Start()
    {
        buttonBar.SetActive(false);
        uiElements = new List<Image>();
        uiElements.Add(transform.Find("0").GetComponent<Image>());
        uiElements.Add(transform.Find("1").GetComponent<Image>());
        uiElements.Add(transform.Find("2").GetComponent<Image>());
        foreach (Image img in uiElements)
        {
            img.enabled = false;
        }
        AviationEventManagerMenu.Instance.onCutSceneEnd += enable;
        AviationEventManagerMenu.Instance.onNextScreen += next;
        AviationEventManagerMenu.Instance.onPrevScreen += prev;
        updateGUI();
    }

    private void enable()
    {
        index = 0;
        uiElements[index].enabled = true;

        buttonBar.SetActive(true);
    }

    private void next()
    {
        if (index + 1 == uiElements.Count) return;
        uiElements[index].enabled = false;
        index++;
        uiElements[index].enabled = true;
        updateGUI();
    }

    private void prev()
    {
        if (index - 1 == -1) return;
        uiElements[index].enabled = false;
        index--;
        uiElements[index].enabled = true;
        updateGUI();
    }

    private void updateGUI()
    {
        updateStars();
        updateStartButton();
    }

    private void updateStartButton() 
    {
        if (index != 1) startGame.SetActive(false);
        else
        {
            startGame.SetActive(true);
            StartLevel startScp = startGame.GetComponentInChildren<StartLevel>();
            startScp.updateLevelIndex(index);
        }

    }

    private void updateStars()
    {
        if (index == 0) rating.SetActive(false);
        else
        {
            rating.SetActive(true);
            StarRatingMenu ratingScp = rating.GetComponentInChildren<StarRatingMenu>();
            ratingScp.updateStars(index);
        }
    }
}
