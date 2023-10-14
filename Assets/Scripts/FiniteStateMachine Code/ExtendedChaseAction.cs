using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableAI/Actions/ExtendedChase")]
public class ExtendedChaseAction : Action
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
        controller.agent.isStopped = false;
        controller.isSleeping = false;
        controller.m_rigidbody.isKinematic = false;
        controller.m_player.transform.gameObject.layer = 9;
        controller.selfFreezeCounter = 0f;
        SetChaseParameters(controller);
        controller.agent.destination = controller.m_player.transform.position;
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
