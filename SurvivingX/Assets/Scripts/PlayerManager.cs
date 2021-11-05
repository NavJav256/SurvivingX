using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    InputManager inputManager;
    PlayerController playerController;
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerController.HandleAllMovement();
    }
}
