using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Goto : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]private bool next;
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData data)
    {
        if(next) AviationEventManagerMenu.Instance.NextScreen();
        else AviationEventManagerMenu.Instance.PrevScreen();
    }
}
