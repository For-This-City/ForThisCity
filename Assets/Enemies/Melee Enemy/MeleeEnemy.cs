using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour, IDamagable, ITargetSetable
{
    NavMeshAgent agent;
    Animator stateMachine;
    public GameObject[] patrolWayPoints;

    public GameObject player;

    public GameObject GetPlayer()
    {
        return player;
    }
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateMachine = GetComponent<Animator>();
    }
    
    void Update()
    {
        stateMachine.SetFloat("distanceToPlayer", Vector3.Distance(transform.position, player.transform.position));
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage");
    }

    public void SetTarget(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public GameObject[] GetPatrolWayPoints()
    {
        return patrolWayPoints;
    }
}
