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

    [Header("Break it")]
    [SerializeField]
    private Slider playerHealth;
    [SerializeField]
    private Slider playerSpeed;
    [SerializeField]
    private Slider playerDamage;
    [SerializeField]
    private Slider playerHealthRegen;
    [SerializeField]
    private Slider playerSprintRegen;
    [SerializeField]
    private Slider playerManaRegen;
    [SerializeField]
    private Slider enemyHealth;
    [SerializeField]
    private Slider enemyDamage;
    [SerializeField]
    private Slider enemySpawnRate;
    [SerializeField]
    private Slider enemySpeed;

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

    //Break it methods...

    public void breakItInitialValues()
    {

    }

    public void setPlayerHealth(float health)
    {
        StateController.playerHealth = (int)health;
    }

    public void setPlayerSpeed(float speed)
    {
        StateController.playerSpeed = (int)speed;
    }

    public void setPlayerDamage(float damage)
    {
        StateController.playerDamage = (int)damage;
    }

    public void setPlayerHealthRegen(float rate)
    {
        StateController.playerHealthRegenRate = rate;
    }

    public void setStaminaRegenRate(float rate)
    {
        StateController.playerStaminaRegenRate = rate;
    }

    public void setManaRegen(float rate)
    {
        StateController.playerHManaRegenRate = rate;
    }

    public void setEnemyHealth(float health)
    {
        StateController.enemyHealth = health;
    }

    public void setEnemySpeed(float speed)
    {
        StateController.enemySpeed = speed;
    }

    public void setEnemyDamage(float damage)
    {
        StateController.enemyDamage = (int)damage;
    }

    public void setSpawnRate(float rate)
    {
        StateController.enemySpawnRate = rate;
    }
}
