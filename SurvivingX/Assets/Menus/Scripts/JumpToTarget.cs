using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpToTarget : MonoBehaviour
{
    public Transform target;

    public float time;

    private void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = target.position;
    }
}
