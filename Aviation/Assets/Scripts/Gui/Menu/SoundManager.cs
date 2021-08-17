using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AudioBuddyTool;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private string key;
    private bool changing;

    private void Update()
    {
        if (key != null && !key.Equals("")) gameObject.GetComponent<Slider>().SetValueWithoutNotify(Options.Instance.getVolumeByKey(key));
    }
    public void change()
    {
        if (key != null && !key.Equals("")) Options.Instance.setVolume(key, gameObject.GetComponent<Slider>().value);
        else Options.Instance.resetAllVolumes();
    }

    public void OnPointerUp(PointerEventData data)
    {
        change();
        AudioBuddy.Play("button_2", Options.Instance.EffectVolume);
        if (!key.Equals("Aviation_EffectVolume"))
        {
            GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>().Speaker = AudioBuddy.Play("Reaching the Sky - Menü", Options.Instance.MusicVolume);
        }
    }
}
