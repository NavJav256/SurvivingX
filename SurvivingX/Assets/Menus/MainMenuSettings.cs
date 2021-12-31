using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSettings : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider playerSensitivitySlider;
    [SerializeField]
    private Slider aimSensitivitySlider;
    [SerializeField]
    private AudioSource backgroundSounds;
    [SerializeField]
    private AudioSource forward;
    [SerializeField]
    private AudioSource back;

    private bool yAxisState = false;

    private void Start()
    {
        StateController.gameSensitivity = 1f;
        StateController.aimSensitivity = 0.5f;
        StateController.invertYAxis = false;
    }

    public void setMasterVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void setGameSensitivity(float sensitivity)
    {
        Debug.Log(sensitivity);
        StateController.gameSensitivity = sensitivity;
        playerSensitivitySlider.value = sensitivity;
    }

    public void setAimSensitivity(float sensitivity)
    {
        StateController.aimSensitivity = sensitivity;
        aimSensitivitySlider.value = sensitivity;
    }

    public void loadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void changeYAxisState()
    {
        if(!yAxisState)
        {
            StateController.invertYAxis = true;
            yAxisState = true;
            return;
        }
        StateController.invertYAxis = false;
        yAxisState = false;
    }

    public void forwardSound()
    {
        
    }

    public void backSound()
    {

    }

    public void quitGame()
    {
        Application.Quit();
    }
}
