using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEvents : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        AviationEventManager.Instance.reset();
    }
}
