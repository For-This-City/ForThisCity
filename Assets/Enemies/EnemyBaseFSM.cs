using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseFSM : StateMachineBehaviour {

	protected GameObject self;
	protected GameObject player;
    protected ITargetSetable targetSetable;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		self = animator.gameObject;
		player = self.GetComponent<MeleeEnemy>().GetPlayer();
        targetSetable = self.GetComponent<ITargetSetable>();
    }
}