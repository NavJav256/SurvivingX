using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{

    private Rigidbody bulletRigid;

    private void Awake()
    {
        bulletRigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigid.velocity = transform.forward  * 50f;
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}
