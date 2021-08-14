using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioBuddyTool;

public class ItemPickupSound : MonoBehaviour
{
    private HashSet<int> collected;
    // Start is called before the first frame update
    void Start()
    {
        collected = new HashSet<int>();
        AviationEventManagerGui.Instance.onBooster += boosterPickup;
        AviationEventManagerGui.Instance.onItemPickup += itemPickup;
    }

    private void boosterPickup()
    {
        AudioBuddy.Play("einsammeln_5", Options.Instance.EffectVolume);
    }
    private void itemPickup(int i)
    {
        if (!collected.Contains(i))
        {
            AudioBuddy.Play("einsammeln_5", Options.Instance.EffectVolume);
            collected.Add(i);
        }
    }
}
