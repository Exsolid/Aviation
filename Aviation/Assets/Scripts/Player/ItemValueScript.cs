using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemValueScript : MonoBehaviour
{
    public int itemValue = 1;

    //Item checks if it has collided with the Player. If yes the Counter gets updated and the Object is destroyed
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            ItemCountScript.instance.ChangeCount(itemValue);
            Destroy(gameObject);
        }
    }
}
