using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    public void setMaxEXP(int xp)
    {
        slider.maxValue = xp;
        slider.value = xp;
    }
    public void setEXP(int xp)
    {
        slider.value = xp;
    }
}
