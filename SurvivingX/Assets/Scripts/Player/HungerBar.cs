using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    public void setMaxHunger(int hunger)
    {
        slider.maxValue = hunger;
        slider.value = hunger;
    }
    public void setHunger(int hunger)
    {
        slider.value = hunger;
    }
}
