using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Slider healthSlider;

    private float damage = StateController.enemyDamage;
    private NavMeshAgent agent;
    private float moveTimer;
    private float changeDirectionTimeLimit = 5f;
    private Animator animator;
    private float lookRadius = StateController.viewRadius;
    private float damageRadius = StateController.damageRadius;
    private float defaultSpeed = 3.5f;
    private float chaseSpeed = StateController.enemyChaseSpeed;


    public PlayerStats playerStats;


    float health = StateController.enemyHealth;
    float maxHealth = StateController.enemyHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        changeWanderingDirection();
        animator = GetComponent<Animator>();
        healthSlider.value = calculateHealth();
        //playerStats = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {

        healthSlider.value = calculateHealth();

        agent.speed = defaultSpeed;

        // Distance from the player to agent (enemy)
        float distance = Vector3.Distance(target.position, transform.position);

        animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), 0f, Time.deltaTime*10f));
        animator.SetLayerWeight(4, Mathf.Lerp(animator.GetLayerWeight(4), 0f, Time.deltaTime * 10f));
        animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime*10f));

        if (distance <= lookRadius)
        {
            agent.destination = target.position;
            agent.isStopped = false;
            agent.speed = chaseSpeed;
            animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), 1f, Time.deltaTime * 10f));
            if (distance <= agent.stoppingDistance)
            {
                faceTarget();
            }
            // Deal damage to the player if within damage radius of 3
            if (distance <= damageRadius)
            {
                playerStats.takingDamage = true;
                //Debug.Log("I am dealing damage");
                animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), 0f, Time.deltaTime * 10f));
                animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 1f, Time.deltaTime * 10f));
            }
            else
            {
                playerStats.takingDamage = false;
                animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 0f, Time.deltaTime * 10f));
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
        float newXDirection = Random.Range(-24.5f, 24.5f);
        float newYDirection = Random.Range(-24.5f, 24.5f);
        agent.destination = (new Vector3(newXDirection, gameObject.transform.position.y, newYDirection));
        agent.isStopped = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        BulletProjectile bullet = collision.gameObject.GetComponent<BulletProjectile>();
        if (collision.gameObject.tag=="Bullet")
        {
            agent.speed = 1f;
            //animator.SetLayerWeight(4, Mathf.Lerp(animator.GetLayerWeight(4), 1f, Time.deltaTime * 10f));
            //StartCoroutine(waiter());
            Debug.Log("Monster has been hit");
            health -= bullet.damage;
            // Enemy has died
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public float calculateHealth()
    {
        return health / maxHealth;
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("does this work?");
    }

}