using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{

    [SerializeField] private GameObject image;
    // Start is called before the first frame update
    void Start()
    {
        image.SetActive(true);
        image.GetComponent<Image>().color = new Color(0, 0, 0, 255);
        StartCoroutine(fade());
    }


    IEnumerator fade()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            image.GetComponent<Image>().color = new Color(0, 0, 0, i);
            yield return null;
        }
    }
}
