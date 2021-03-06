using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AudioBuddyTool;
public class Rebinding : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string control;
    [SerializeField] private string actionName;
    private string keyPress;
    private bool isKeyboard;
    private bool isSetting;
    private string alternateText;

    private Text textChild;
    void Start()
    {
        alternateText = "";
        textChild = (Text)gameObject.GetComponentInChildren(typeof(Text));
    }
    public void OnGUI()
    {
        textChild.text = alternateText.Equals("") ? returnKeyCode(Options.Instance.currentValueOfControl(control)) : alternateText;
        Event e = Event.current;
        if (e != null && e.type.Equals(EventType.KeyDown) && e.keyCode != KeyCode.None)
            keyPress = e.keyCode.ToString(); isKeyboard = true;
        if (e != null && e.isMouse)
        {
            isKeyboard = false;
            switch (e.button)
            {
                case 0:
                    keyPress = "leftButton";
                    break;
                case 1:
                    keyPress = "rightButton";
                    break;
                case 2:
                    keyPress = "middleButton";
                    break;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (!isSetting)
        {
            AudioBuddy.Play("button_2", Options.Instance.EffectVolume);
            StartCoroutine(setEvent());
            isSetting = true;
        }
    }

    private void setKeyToControl(string key, string keyOrMouseCode)
    {
        alternateText = "";
        string keyPath = keyOrMouseCode + key;
        Dictionary<string, string> temp = new Dictionary<string, string>();
        if(!Options.Instance.setKey(control, keyPath, actionName))
        {
            alternateText = "Input invalid";
            StartCoroutine(resetText());
        }
    }
    IEnumerator resetText()
    {
        yield return new WaitForSeconds(3);
        alternateText = "";
    }
        IEnumerator setEvent()
    {
        keyPress = "";
        do
        {
            yield return null;
        } while (keyPress == "");
        setKeyToControl(keyPress, isKeyboard ? "<Keyboard>/" : "<Mouse>/");
        yield return new WaitForSeconds(0.3f);
        isSetting = false;
    }

    private string returnKeyCode(string path)
    {
        if (path.Equals("")) return path;
        char[] chars = path.ToCharArray();
        bool canStart = false;
        path = "";
        bool nextToUpper = true;
        foreach (char c in chars)
        {
            if (canStart)
            {
                char newC = c;
                if (nextToUpper)
                {
                    newC = char.ToUpper(newC);
                    nextToUpper = false;
                }
                else if (char.IsUpper(newC)) path = path + " ";
                path = path + newC;

            }
            if (c.Equals('/')) canStart = true;
        }
        return path;
    }
}
