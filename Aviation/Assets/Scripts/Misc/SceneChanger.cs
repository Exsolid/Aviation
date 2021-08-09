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
        if(ids.Count >= 5)
            SceneManager.LoadScene("WinningScreen");
    }
}
