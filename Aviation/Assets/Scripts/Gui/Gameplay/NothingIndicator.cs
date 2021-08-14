using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NothingIndicator : MonoBehaviour
{
    private bool listen;
    private bool blink;
    private float timer;
    private bool item;
    [SerializeField] private float blinkInterval;
    void Start()
    {
        item = false;
        AviationEventManagerGui.Instance.onCloudEnter += startListen;
        AviationEventManagerGui.Instance.onCloudExit += stopListen;
        AviationEventManagerGui.Instance.onBooster += setItem;
        AviationEventManagerGui.Instance.onItemPickup += setItem;
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
        indicate();
        item = false;
    }

    private void indicate()
    {
        if (!item)
        {
            blink = true;
            StartCoroutine(blinkTimer());
        }
    }

    private void setItem(int i)
    {
        if(listen)
             item = true;
    }

    private void setItem()
    {
        if (listen)
            item = true;
    }

    IEnumerator blinkTimer()
    {
        yield return new WaitForSeconds(4);
        blink = false;
        if (gameObject != null) gameObject.GetComponent<Image>().enabled = true;
    }
}
