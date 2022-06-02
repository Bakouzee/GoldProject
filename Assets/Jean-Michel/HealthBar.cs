using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);

    }

    public void SetHealth(int health, int healthMax)
    {
        if (slider.maxValue != healthMax)
            slider.maxValue = healthMax;
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    
}
