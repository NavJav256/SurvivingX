using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpToTarget : MonoBehaviour
{
    public Transform target;

    public float time;

    public Animator animator;
    NavMeshAgent agent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        agent = GetComponent<NavMeshAgent>();
        
    }

    private void Update()
    {
        agent.speed = 2f;
        agent.destination = target.position;
        animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), 1f, Time.deltaTime * 10f));
    }
}
