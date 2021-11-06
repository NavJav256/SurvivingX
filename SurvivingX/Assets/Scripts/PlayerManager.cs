using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    InputManager inputManager;
    PlayerController playerController;
    CameraManager cameraManager;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerController = GetComponent<PlayerController>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
        cameraManager.HandleAllCameraMovement();
    }

    private void FixedUpdate()
    {
        playerController.HandleAllMovement();
    }

    private void LateUpdate()
    {
        
    }
}
