using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using AudioBuddyTool;
public class Init : MonoBehaviour
{
    [SerializeField] private InputActionAsset controls;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Aviation_Controls")) controls.LoadFromJson(PlayerPrefs.GetString("Aviation_Controls"));
        Options.Instance.setControls(controls);
    }
}
