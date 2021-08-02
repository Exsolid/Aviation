using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private string key;

    private void Update()
    {
        if (key != null && !key.Equals("")) gameObject.GetComponent<Slider>().SetValueWithoutNotify(Options.Instance.getVolumeByKey(key));
    }
    public void change()
    {
        if (key != null && !key.Equals("")) Options.Instance.setVolume(key, gameObject.GetComponent<Slider>().value);
        else Options.Instance.resetAllVolumes();
    }
}
