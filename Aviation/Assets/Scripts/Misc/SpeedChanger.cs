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
                AviationEventManager.Instance.CloudEnter();
                scp.reduceSpeed();
            }
            else
            {
                AviationEventManager.Instance.Booster();
                scp.increaseSpeed();
            }
            if (deleteOnTouch) Destroy(gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        AviationEventManager.Instance.CloudExit();
    }
}
