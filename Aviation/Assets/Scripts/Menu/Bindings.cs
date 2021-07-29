using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public class Bindings
{
    private Dictionary<string, string> initConToKey;
    private Dictionary<string, string> currentConToKey;
    private InputActionAsset c;
    private static Bindings instance;
    public static Bindings Instance { 
        get {
            if (instance == null) instance = new Bindings();
            return instance;
    } }

    public Bindings()
    {
        initConToKey = new Dictionary<string, string>();
        currentConToKey = new Dictionary<string, string>();
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
        writeToPlayerPrefs();
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
        writeToPlayerPrefs();
    }

    public void resetAll()
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
        writeToPlayerPrefs();
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

    private void writeToPlayerPrefs()
    {
        PlayerPrefs.SetString("Controls", c.ToJson());
    }
}
