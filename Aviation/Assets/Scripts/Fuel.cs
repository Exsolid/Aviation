using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fuel : MonoBehaviour
{
    public Slider slider;

    //Maximum of Fuel is set and Slider is adjusted
    public void SetMaxFuel(float fuel)
    {
        slider.maxValue = fuel;
        slider.value = fuel;
    }

    //Slider reacts to value change
    public void SetFuel(float fuel)
    {
        slider.value = fuel;
    }
}
