using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    Transform target;
    NavMeshAgent agent;
    float moveTimer;
    float changeDirectionTimeLimit = 5f;

    public float lookRadius = 15f;
    public float damageRadius = 5f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        changeWanderingDirection();
    }

    // Update is called once per frame
    void Update()
    {
        // Distance from the player to agent (enemy)
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            if (distance <= agent.stoppingDistance)
            {
                faceTarget();
            }
            // Deal damage to the player if within damage radius of 3
            if (distance <= damageRadius)
            {
                Debug.Log("I am dealing damage");
            }
        }

        // Every 5 seconds change the direction the NPC travels
        if (moveTimer >= changeDirectionTimeLimit)
        {
            moveTimer = 0;
            changeWanderingDirection();
        }

        // Increase timer by 1 second
        moveTimer += Time.deltaTime;
    }

    // Draw radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }

    // Point towards the player
    void faceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);
    }

    // Change random direction
    void changeWanderingDirection()
    {
        float newXDirection = Random.Range(1, 500);
        float newYDirection = Random.Range(1, 500);
        agent.SetDestination(new Vector3(newXDirection, gameObject.transform.position.y, newYDirection));
    }

}