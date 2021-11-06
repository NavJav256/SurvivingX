using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    InputManager inputManager;


    [SerializeField]
    Transform cameraPivot;
    Transform cameraTransform;
    Transform targetTransform;

    Vector3 cameraFollowVelocity = Vector3.zero;
    Vector3 cameraVectorPosition;

    float defaultPosition;

    [SerializeField]
    float cameraFollowSpeed = 0.2f;
    [SerializeField]
    float cameraLookSpeed = 0.1f;
    [SerializeField]
    float cameraPivotSpeed = 0.1f;
    [SerializeField]
    float cameraCollisionRadius = 0.2f;
    [SerializeField]
    float cameraCollisionOffset = 0.2f;
    [SerializeField]
    float minCollisionOffset = 0.2f;
    [SerializeField]
    LayerMask collisionLayers;

    float lookAngle;
    float pivotAngle;
    float minPivotAngle = -20;
    float maxPivotAngle = 20;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle += inputManager.cameraInputX * cameraLookSpeed;
        pivotAngle -= inputManager.cameraInputY * cameraPivotSpeed;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if(Physics.SphereCast
            (cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition -= distance - cameraCollisionOffset;
        }

        if(Mathf.Abs(targetPosition) < minCollisionOffset)
        {
            targetPosition -= minCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
