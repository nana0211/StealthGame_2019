using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{

    public float ChaseSpeed = 3f;
    public float ChaseAngularSpeed = 220f;
    public float ChaseAcceleration = 0.6f;
    public float ChaseStoppingDistance = 0f;

    public override void Act(StateController controller)
    {
        Chase(controller);
        //Debug.Log("chasing now");
    }

    private void Chase(StateController controller)
    {
        
        SetChaseParameters(controller);
        controller.agent.destination = controller.chaseTarget.position;
        //Debug.Log(controller.m_AgentID + "chase at " + controller.agent.destination);
    }


    private void SetChaseParameters(StateController controller)
    {
        // Define Chase Speed Parameters
        controller.agent.speed = ChaseSpeed;
        controller.agent.angularSpeed = ChaseAngularSpeed;
        controller.agent.acceleration = ChaseAcceleration;
        controller.agent.stoppingDistance = ChaseStoppingDistance;
    }
}
