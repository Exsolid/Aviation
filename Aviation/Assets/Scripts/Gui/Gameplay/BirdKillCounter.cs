using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdKillCounter : MonoBehaviour
{
    private int count;
    public int Count { get { return count; } }
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        AviationEventManagerGui.Instance.onBirdKill += updateGui;
    }

    private void updateGui()
    {
        count++;
        gameObject.GetComponent<Text>().text = "x" + count;
    }
}
