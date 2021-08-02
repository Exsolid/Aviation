using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Options
{
    private Dictionary<string, string> initConToKey;
    private Dictionary<string, string> currentConToKey;
    private InputActionAsset c;

    private Dictionary<string, float> initVolume;
    private Dictionary<string, float> currentVolume;
    public float MusicVolume { get { return currentVolume["Aviation_MusicVolume"] / 100 * currentVolume["Aviation_MasterVolume"] / 100; } }
    public float EffectVolume { get { return currentVolume["Aviation_EffectVolume"] / 100 * currentVolume["Aviation_MasterVolume"] / 100; } }
    public float MasterVolume { get { return currentVolume["Aviation_EffectVolume"] / 100; } }

    private static Options instance;
    public static Options Instance { 
        get {
            if (instance == null) instance = new Options();
            return instance;
    } }

    public Options()
    {
        initConToKey = new Dictionary<string, string>();
        currentConToKey = new Dictionary<string, string>();
        initVolume = new Dictionary<string, float>();
        currentVolume = new Dictionary<string, float>();

        initVolume.Add("Aviation_MasterVolume", PlayerPrefs.HasKey("Aviation_MasterVolume") ? PlayerPrefs.GetFloat("Aviation_MasterVolume") : 50);
        initVolume.Add("Aviation_EffectVolume", PlayerPrefs.HasKey("Aviation_EffectVolume") ? PlayerPrefs.GetFloat("Aviation_EffectVolume") : 100);
        initVolume.Add("Aviation_MusicVolume", PlayerPrefs.HasKey("Aviation_MusicVolume") ? PlayerPrefs.GetFloat("Aviation_MusicVolume") : 100);
        currentVolume.Add("Aviation_MasterVolume", PlayerPrefs.HasKey("Aviation_MasterVolume") ? PlayerPrefs.GetFloat("Aviation_MasterVolume") : 50);
        currentVolume.Add("Aviation_EffectVolume", PlayerPrefs.HasKey("Aviation_EffectVolume") ? PlayerPrefs.GetFloat("Aviation_EffectVolume") : 100);
        currentVolume.Add("Aviation_MusicVolume", PlayerPrefs.HasKey("Aviation_MusicVolume") ? PlayerPrefs.GetFloat("Aviation_MusicVolume") : 100);
    }

    public bool setKey(string control, string path, string actionName)
    {
        control = stripToEmpty(control);
        if (currentConToKey.ContainsValue(path)) return false;
        if (control.Equals(""))
        {
            //Workaround for InputSystem bug (else part does not set the control to right binding if the name is empty)
            InputAction ac = c.FindActionMap("Gameplay").FindAction("Shoot");
            ac.ChangeBinding(0).WithPath(path);
            currentConToKey[control] = path;
        }
        else
        {
            InputAction ac = c.FindAction(actionName);
            ac.ChangeBindingWithPath(currentConToKey[control]).WithPath(path);
            currentConToKey[control] = path;
        }
        writeToControlsPlayerPrefs();
        return true;
    }
    public void resetKey(string control, string actionName)
    {
        if (control.Equals(""))
        {
            //Workaround for InputSystem bug (else part does not set the control to right binding if the name is empty)
            control = stripToEmpty(control);
            InputAction ac = c.FindActionMap("Gameplay").FindAction("Shoot");
            ac.ChangeBinding(0).WithPath(initConToKey[control]);
            currentConToKey[control] = initConToKey[control];
            Debug.Log(ac.actionMap.ToJson());
        }
        else
        {
            control = stripToEmpty(control);
            InputAction ac = c.FindAction(actionName);
            ac.ChangeBindingWithPath(currentConToKey[control]).WithPath(initConToKey[control]);
            currentConToKey[control] = initConToKey[control];
        }
        writeToControlsPlayerPrefs();
    }

    public void resetAllKeys()
    {
        InputAction ac = c.FindAction("Movement");
        foreach (InputBinding bc in c.FindActionMap("Gameplay").FindAction("Movement").bindings)
        {
            if (!bc.name.Equals("Controls"))
            {
                ac.ChangeBindingWithPath(currentConToKey[bc.name]).WithPath(initConToKey[bc.name]);
                currentConToKey[bc.name] = initConToKey[bc.name];
            }
        }
        ac = c.FindAction("Shoot");
        ac.ChangeBinding(0).WithPath(initConToKey[stripToEmpty(ac.bindings[0].name)]);
        currentConToKey[stripToEmpty(ac.bindings[0].name)] = initConToKey[stripToEmpty(ac.bindings[0].name)];
        writeToControlsPlayerPrefs();
    }

    public string currentValueOfControl(string control)
    { 
        control = stripToEmpty(control);
        return currentConToKey[control];
    }
    private string stripToEmpty(string str)
    {
        return str == null ? "" : str;
    }

    public void setControls(InputActionAsset c)
    {
        this.c = c;

        foreach (InputBinding bc in c.FindActionMap("Gameplay").FindAction("Movement").bindings)
        {
            initConToKey.Add(bc.name, bc.path);
            currentConToKey.Add(bc.name, bc.path);
        }
        initConToKey.Add(stripToEmpty(c.FindActionMap("Gameplay").FindAction("Shoot").bindings[0].name), c.FindActionMap("Gameplay").FindAction("Shoot").bindings[0].path);
        currentConToKey.Add(stripToEmpty(c.FindActionMap("Gameplay").FindAction("Shoot").bindings[0].name), c.FindActionMap("Gameplay").FindAction("Shoot").bindings[0].path);
    }

    private void writeToControlsPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("Controls");
        PlayerPrefs.SetString("Aviation_Controls", c.ToJson());
    }

    public void setVolume(string key, float value)
    {
        if (currentVolume.ContainsKey(key))
        {
            currentVolume[key] = value;
            PlayerPrefs.SetFloat(key, value);
        }
    }

    public float getVolumeByKey(string key)
    {
        if (currentVolume.ContainsKey(key))
        {
            return currentVolume[key];
        }
        return 0;
    }

    public void resetAllVolumes()
    {
        foreach(string key in initVolume.Keys)
        {
            currentVolume[key] = initVolume[key];
            PlayerPrefs.SetFloat(key, initVolume[key]);
        }
    }
}
