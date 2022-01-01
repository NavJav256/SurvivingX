using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject PauseMenuUI;
    public GameObject crosshair;

    //public StarterAssetsInputs starter;

    private void Awake()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        crosshair.SetActive(true);
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        crosshair.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void returnToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void buttonPress()
    {
        Debug.Log("test");
    }
}

