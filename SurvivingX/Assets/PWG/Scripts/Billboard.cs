using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public Transform mainCamera;
    
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.forward);
    }
}
