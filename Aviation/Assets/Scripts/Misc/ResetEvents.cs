using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AviationEventManager.Instance.reset();
    }
}
