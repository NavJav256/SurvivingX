using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{

    Animator animator;
    int horizontal;
    int vertical;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimValues(float horizontalMovement, float verticalMovement, bool isSprinting, bool isWalking)
    {

        float snappedHorizontal;
        float snappedVertical;

        if (horizontalMovement > 0 && horizontalMovement < 0.55f) snappedHorizontal = 0.5f;
        else if (horizontalMovement > 0.55f) snappedHorizontal = 1;
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f) snappedHorizontal = -0.5f;
        else if (horizontalMovement < -0.55f) snappedHorizontal = -1;
        else snappedHorizontal = 0;

        if (verticalMovement > 0 && verticalMovement < 0.55f) snappedVertical = 0.5f;
        else if (verticalMovement > 0.55f) snappedVertical = 1;
        else if (verticalMovement < 0 && verticalMovement > -0.55f) snappedVertical = -0.5f;
        else if (verticalMovement < -0.55f) snappedVertical = -1;
        else snappedVertical = 0;

        if(isSprinting && verticalMovement != 0)
        {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2;
        }
        if(isWalking && verticalMovement != 0)
        {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 0.5f;
        }


        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }
}
