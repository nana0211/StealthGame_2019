using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingScript : StateMachineBehaviour
{

    public GameObject waypoints;

    public float minWaitTime;
    public float maxWaitTime;

    private NavMeshAgent agent;
    private Vector3 curr_destination;
    private List<GameObject> waypoint_list;


    private bool IsWalking = false;
    private bool IsWaiting = false;

    private float CurrentWaitTimer = 0f;
    private float MaxWaitTimer = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waypoint_list = new List<GameObject>();
        //agent = GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;
        //getWaypoints(waypoints);
        curr_destination = Vector3.zero;

        // Initialize a Destination
        //SetNewRandomDestination();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
