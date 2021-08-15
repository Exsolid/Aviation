using AudioBuddyTool;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonSound : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler
{
    public void OnPointerUp(PointerEventData data)
    {
        AudioBuddy.Play("button_2", Options.Instance.EffectVolume);
    }
    public void OnPointerEnter(PointerEventData data)
    {
        AudioBuddy.Play("button_hover", Options.Instance.EffectVolume);
    }
} 

