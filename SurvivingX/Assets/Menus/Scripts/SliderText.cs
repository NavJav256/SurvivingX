using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{

    public TextMesh textComponent;

    void Start()
    {
        
    }

    public void SetSliderValue(float sliderValue)
    {
        textComponent.text = "PlayerHealth: "+Mathf.Round(sliderValue * 100).ToString();
    }
}
