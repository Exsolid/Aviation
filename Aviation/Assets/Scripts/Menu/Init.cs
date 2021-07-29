using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Init : MonoBehaviour
{
    [SerializeField] private InputActionAsset controls;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("Controls")) controls.LoadFromJson(PlayerPrefs.GetString("Controls"));
        Bindings.Instance.setControls(controls);
    }
}
