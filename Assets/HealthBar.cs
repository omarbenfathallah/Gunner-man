using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth ( int Helath)
    {
        slider.maxValue = Helath;
        slider.value = Helath;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
