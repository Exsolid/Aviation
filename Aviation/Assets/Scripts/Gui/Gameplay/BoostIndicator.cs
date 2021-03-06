using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostIndicator : MonoBehaviour
{
    private bool listen;
    private bool blink;
    private float timer;
    [SerializeField] private float blinkInterval;
    void Start()
    {
        AviationEventManagerGui.Instance.onCloudEnter += startListen;
        AviationEventManagerGui.Instance.onCloudExit += stopListen;
        AviationEventManagerGui.Instance.onBooster += indicate;
        timer = blinkInterval;
    }

    private void Update()
    {
        if (blink)
        {
            timer += Time.deltaTime;
            if (timer > blinkInterval)
            {
                gameObject.GetComponent<Image>().enabled = !gameObject.GetComponent<Image>().enabled;
                timer = 0;
            }
        }
    }

    private void startListen()
    {
        listen = true;
    }

    private void stopListen()
    {
        listen = false;
    }

    private void indicate()
    {
        if (listen)
        {
            blink = true;
            StartCoroutine(blinkTimer());
        }
    }

    IEnumerator blinkTimer()
    {
        yield return new WaitForSeconds(4);
        blink = false;
        if(gameObject != null) gameObject.GetComponent<Image>().enabled = true;
    }
}
