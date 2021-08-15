using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GotoMainMenu : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        AviationEventManagerMenu.Instance.reset();
        SceneManager.LoadScene("MenuMain");
    }
}
