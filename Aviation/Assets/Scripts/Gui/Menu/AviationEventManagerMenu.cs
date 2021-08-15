using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AviationEventManagerMenu
{
    private static AviationEventManagerMenu instance;
    public static AviationEventManagerMenu Instance { get { if (instance == null) instance = new AviationEventManagerMenu(); return instance; } }

    public event Action onCutSceneEnd;
    public event Action onNextScreen;
    public event Action onPrevScreen;

    public void CutSceneEnd()
    {
        if(onCutSceneEnd != null)
        {
            onCutSceneEnd();
        }
    }
    public void NextScreen()
    {
        if (onNextScreen != null)
        {
            onNextScreen();
        }
    }
    public void PrevScreen()
    {
        if (onPrevScreen != null)
        {
            onPrevScreen();
        }
    }
    public void reset()
    {
        onCutSceneEnd = null;
        onNextScreen = null;
        onPrevScreen = null;
    }
}
