using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;

    Rigidbody rb;

    [Header("Falling")]
    [SerializeField]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingSpeed;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    [SerializeField]
    public bool isSprinting;
    public bool isWalking;
    public bool isGrounded;

    [Header("Movement Speeds")]
    [SerializeField]
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

        if (isSprinting)
        {
            moveDirection *= sprintingSpeed;
        }
        else if (isWalking)
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

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y += rayCastHeightOffset;

        if(!isGrounded)
        {
            inAirTimer += Time.deltaTime;
            //rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(Vector3.down * fallingSpeed * 2500 * inAirTimer);
        }

        if(Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, 0.1f, groundLayer))
        {
            if(!isGrounded)
            {
                inAirTimer = 0;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        HandleMovement();
        HandleRotation();
    }
}
