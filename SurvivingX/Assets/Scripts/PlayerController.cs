using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;

    Rigidbody rb;

    public bool isSprinting;
    public bool isWalking;

    [Header("Movement Speeds")]
    float walkingSpeed = 4;
    float runningSpeed = 7;
    float sprintingSpeed = 10;
    float rotationSpeed = 15;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }
    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (rb.velocity.y < 0)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        //moveDirection *= isSprinting ? sprintingSpeed : isWalking ? walkingSpeed : runningSpeed;

        if (isSprinting)
        {
            moveDirection *= sprintingSpeed;
        }
        if (isWalking)
        {
            moveDirection *= walkingSpeed;
        }
        else
        {
            moveDirection *= runningSpeed;
        }
        

        Vector3 moveVelocity = moveDirection;
        rb.velocity = moveVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection += cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }
}
