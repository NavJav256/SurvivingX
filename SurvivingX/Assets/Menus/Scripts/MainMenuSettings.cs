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

        int currentRes = 0;

        for(int i=0; i<screenResolutions.Length; i++)
        {
            resOptions.Add(screenResolutions[i].width + " x " + screenResolutions[i].height);

            if(screenResolutions[i].width == Screen.currentResolution.width && screenResolutions[i].height == Screen.currentResolution.height)
            {
                currentRes = i;
            }
        }

        resDropDown.AddOptions(resOptions);
        resDropDown.value = currentRes;
        resDropDown.RefreshShownValue();
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

    //We still need to add variables for how quick his mana regens etc stuff like that

    public void easyGame()
    {
        StateController.enemyChaseSpeed = 5f;
        StateController.viewRadius = 5f;
        StateController.damageRadius = 2f;
        StateController.enemySpeed = 4f;
        StateController.damage = 3;
        loadGame();
    }

    public void mediumGame()
    {
        StateController.enemyChaseSpeed = 5f;
        StateController.viewRadius = 5f;
        StateController.damageRadius = 3f;
        StateController.enemySpeed = 6f;
        StateController.damage = 5;
        loadGame();
    }

    public void hardGame()
    {
        StateController.enemyChaseSpeed = 9f;
        StateController.viewRadius = 15f;
        StateController.damageRadius = 4f;
        StateController.enemySpeed = 9f;
        StateController.damage = 7;
        loadGame();
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void setFullScreen(bool fullScreenSate)
    {
        Screen.fullScreen = fullScreenSate;
    }

    public void setScreenResolution(int resIndex)
    {
        Resolution res = screenResolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}
