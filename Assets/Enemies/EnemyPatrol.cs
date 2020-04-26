using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : EnemyBaseFSM {

	GameObject[] waypoints;
	int currentWP;
    public float wayPointStopDistance = 2f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		base.OnStateEnter(animator,stateInfo,layerIndex);
        waypoints = targetSetable.GetPatrolWayPoints();
        currentWP = 0;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		if(waypoints.Length == 0) return;

		if(Vector3.Distance(waypoints[currentWP].transform.position, self.transform.position) < wayPointStopDistance)
		{
			currentWP++;
			if(currentWP >= waypoints.Length)
			{
				currentWP = 0;
			}	
		}
        
        targetSetable.SetTarget(waypoints[currentWP].transform.position);
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}

}
