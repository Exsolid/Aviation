using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private HashSet<int> ids;
    void Start()
    {
        ids = new HashSet<int>();
        AviationEventManager.Instance.onItemPickup += countItem;
    }

    private void countItem(int id)
    {
        ids.Add(id);
        Debug.Log(ids.Count);
        if (ids.Count >= 5)
        {
            AviationEventManager.Instance.Win();
            SceneManager.LoadScene("WinningScreen");
        }
    }
}
