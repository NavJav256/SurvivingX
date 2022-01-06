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
    [SerializeField]
    private Dropdown resDropDown;

    private bool yAxisState = false;

    Resolution[] screenResolutions;

    private void Start()
    {
        StateController.gameSensitivity = 1f;
        StateController.aimSensitivity = 0.5f;
        StateController.invertYAxis = false;
        AudioListener.volume = 1f;

        screenResolutions = Screen.resolutions;

        resDropDown.ClearOptions();

        List<string> resOptions = new List<string>();

        for(int i=0; i<screenResolutions.Length; i++)
        {
            resOptions.Add(screenResolutions[i].width + " x " + screenResolutions[i].height);
        }

        resDropDown.AddOptions(resOptions);
    }

    public void changeVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void setMasterVolume(float volume)
    {
        //audioMixer.SetFloat("Volume", volume);
        AudioListener.volume = volume;
    }

    public void setBackgroundVolume(float volume)
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

    public void setFullScreen(bool fullScreenSate)
    {
        Screen.fullScreen = fullScreenSate;
    }
}
