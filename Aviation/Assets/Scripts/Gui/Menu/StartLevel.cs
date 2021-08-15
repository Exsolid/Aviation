using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour, IPointerClickHandler
{
    private int index;
    public void updateLevelIndex(int index)
    {
        this.index = index;
    }

    public void OnPointerClick(PointerEventData data)
    {
        AviationEventManagerMenu.Instance.reset();
        switch (index)
        {
            case 0:
                break;
            case 1:
                SceneManager.LoadScene("Spitfire-Level");
                break;
            case 2:
                break;
        }
    }
}
