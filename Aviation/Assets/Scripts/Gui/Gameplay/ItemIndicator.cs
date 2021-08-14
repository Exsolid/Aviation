using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIndicator : MonoBehaviour
{
    private bool listen;
    private bool blink;
    private float timer;
    private HashSet<int> ids;
    [SerializeField] private float blinkInterval;
    void Start()
    {
        AviationEventManagerGui.Instance.onCloudEnter += startListen;
        AviationEventManagerGui.Instance.onCloudExit += stopListen;
        AviationEventManagerGui.Instance.onItemPickup += indicate;
        timer = blinkInterval;
        ids = new HashSet<int>();
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

    private void indicate(int i)
    {
        if (!ids.Contains(i))
        {
            if (listen)
            {
                blink = true;
                StartCoroutine(blinkTimer());
            }
            ids.Add(i);
        }
    }

    IEnumerator blinkTimer()
    {
        yield return new WaitForSeconds(4);
        blink = false;
        if (gameObject != null) gameObject.GetComponent<Image>().enabled = true;
    }
}
