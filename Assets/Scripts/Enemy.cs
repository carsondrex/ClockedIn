using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D target;
    NavMeshAgent agent;
    public string state = "Idle";
    public float speed;
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (state == "Idle") {
            agent.speed = 0;
        } else {
            agent.speed = speed;
        }
        agent.SetDestination(target.position);
    }
}
