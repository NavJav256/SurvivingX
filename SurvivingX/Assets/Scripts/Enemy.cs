using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    Transform target;
    NavMeshAgent agent;
    public float lookRadius = 10f;
    public float moveTimer;
    public float changeDirectionTimeLimit = 5f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        changeWanderingDirection();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        
        if(distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            if (distance <= agent.stoppingDistance)
            {
                FaceTowardsTarget();
            }
        }

        if (moveTimer >= changeDirectionTimeLimit)
        {
            moveTimer = 0;
            changeWanderingDirection();
        }

        moveTimer += Time.deltaTime;
    }

    // Point towards the player
    void FaceTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void changeWanderingDirection()
    {
        float newXDirection = Random.Range(1, 500);
        float newYDirection = Random.Range(1, 500);
        agent.SetDestination(new Vector3(newXDirection, gameObject.transform.position.y, newYDirection));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}