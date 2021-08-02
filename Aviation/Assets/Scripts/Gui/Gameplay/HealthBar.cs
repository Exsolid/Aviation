using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    //Maximum of health bar is set and slider is adjusted
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    //slider reacts to value change and changes color depending on value
    public void SetHealth(int health)
    {
        slider.value = health;
        
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
