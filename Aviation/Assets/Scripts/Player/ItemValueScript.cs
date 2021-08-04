using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioBuddyTool;

public class ItemValueScript : MonoBehaviour
{
    public int itemValue = 1;

    //Item checks if it has collided with the Player. If yes the Counter gets updated and the Object is destroyed
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            AudioBuddy.Play("einsammeln_5", Options.Instance.EffectVolume);
            ItemCountScript.instance.ChangeCount(itemValue);
            Destroy(gameObject);
        }
    }
}
