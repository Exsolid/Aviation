using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChanger : MonoBehaviour
{
    [SerializeField] private bool negativ;
    [SerializeField] private bool deleteOnTouch;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerBehaviourScript scp = other.GetComponent<PlayerBehaviourScript>();
            if (negativ)
            {
                if(!deleteOnTouch) AviationEventManagerGui.Instance.CloudEnter();
                scp.reduceSpeed();
            }
            else
            {
                AviationEventManagerGui.Instance.Booster();
                scp.increaseSpeed();
            }
            if (deleteOnTouch) Destroy(gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !deleteOnTouch)
        {
            AviationEventManagerGui.Instance.CloudExit();
        }
    }
}
