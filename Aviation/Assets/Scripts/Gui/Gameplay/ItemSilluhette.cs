
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSilluhette : MonoBehaviour
{
    [SerializeField]private int id;
    public int ID { get { return id; } set { id = value; } }
    // Start is called before the first frame update
    void Start()
    {
        AviationEventManagerGui.Instance.onItemPickup += updateGui;
        gameObject.transform.Find("Part").gameObject.SetActive(false);
        gameObject.transform.Find("Sillhuette").gameObject.SetActive(true);
    }

    private void updateGui(int id)
    {
        if (id == this.id)
        {
            gameObject.transform.Find("Part").gameObject.SetActive(true);
            gameObject.transform.Find("Sillhuette").gameObject.SetActive(false);
        }
    }
}
