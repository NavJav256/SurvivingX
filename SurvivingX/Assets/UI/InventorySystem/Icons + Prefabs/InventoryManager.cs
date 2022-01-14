using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject inventory;
    public GameObject HUD;

    //public StarterAssetsInputs starter;

    private void Awake()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
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
        inventory.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        HUD.SetActive(true);
    }

    public void Pause()
    {
        inventory.SetActive(true);
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

