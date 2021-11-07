using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerController playerController;
    AnimatorManager animManager;

    [SerializeField]
    Vector2 movementInput;

    [SerializeField]
    Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool shiftInput;
    public bool ctrlInput;
    public bool spaceInput;

    private void Awake()
    {
        animManager = GetComponent<AnimatorManager>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Player.Look.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.Player.Sprint.performed += i => shiftInput = true;
            playerControls.Player.Sprint.canceled += i => shiftInput = false;

            playerControls.Player.Walk.performed += i => ctrlInput = true;
            playerControls.Player.Walk.canceled += i => ctrlInput = false;

            playerControls.Player.Jump.performed += i => spaceInput = true;
        }

        playerControls.Enable();

    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void HandleJumpInput()
    {
        if(spaceInput)
        {
            spaceInput = false;
            playerController.HandleJump();
        }
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animManager.UpdateAnimValues(0, moveAmount, playerController.isSprinting, playerController.isWalking);
    }

    private void HandleSprintingInput()
    {
        if(shiftInput)
        {
            playerController.isSprinting = true;
        } 
        else
        {
            playerController.isSprinting = false;
        }
    }

    private void HandleWalkingInput()
    {
        if (ctrlInput)
        {
            playerController.isWalking = true;
        }
        else
        {
            playerController.isWalking = false;
        }
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleWalkingInput();
        HandleJumpInput();
    }
}
