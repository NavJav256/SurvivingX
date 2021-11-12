using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    InputManager inputManager;
    PlayerController playerController;
    CameraManager cameraManager;
    Animator animator;

    public bool isInteracting;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        playerController = GetComponent<PlayerController>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerController.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        playerController.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerController.isGrounded);
    }
}
