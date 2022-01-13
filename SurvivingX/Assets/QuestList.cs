using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class QuestList : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject qList;
    public GameObject HUD;

    //public StarterAssetsInputs starter;

    private void Awake()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
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
        qList.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        HUD.SetActive(true);
    }

    public void Pause()
    {
        qList.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        HUD.SetActive(false);
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

