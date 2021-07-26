using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Rebinding : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string control;
    [SerializeField] public InputActionAsset controls;
    private Dictionary<string, string> conToKey;
    private InputActionMap map;
    private InputAction ac;
    private string keyPress;

    private Text textChild;
    void Start()
    {
        textChild = (Text)gameObject.GetComponentInChildren(typeof(Text));
        map = controls.FindActionMap("Gameplay");
        conToKey = new Dictionary<string, string>();
        ac = map.FindAction("Movement");
        foreach (InputBinding b in ac.bindings)
        {
            conToKey.Add(b.name, b.path);
        }
        textChild.text = returnKeyCode(conToKey[control]);
    }
    public void OnGUI()
    {
        Event e = Event.current;
        if (e != null && e.type.Equals(EventType.KeyDown) && e.keyCode != KeyCode.None)
            keyPress = e.keyCode.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(map.ToJson());
        StartCoroutine(setEvent());
    }
    private void setKeyToControl(string key)
    {
        Debug.Log(key);
        key = "<Keyboard>/" + key;

        ac.ChangeBindingWithPath(conToKey[control]).WithPath(key);
        conToKey[control] = key;
        textChild.text = returnKeyCode(conToKey[control]);
        Debug.Log(map.ToJson());
    }

    IEnumerator setEvent()
    {
        keyPress = "";
        do
        {
            yield return null;
        } while (keyPress == "");
        setKeyToControl(keyPress);
    }

    private string returnKeyCode(string path)
    {
        if (path.Equals("")) return path;
        char[] chars = path.ToCharArray();
        bool canStart = false;
        path = "";
        foreach (char c in chars)
        {
            if (canStart)
            {
                path = path + c;
            }
            if (c.Equals('/')) canStart = true;
        }
        return path;
    }
}
