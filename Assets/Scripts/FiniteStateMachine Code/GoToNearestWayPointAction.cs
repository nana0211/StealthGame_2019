using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableAI/Actions/GoToNearestWayPointAction")]
public class GoToNearestWayPointAction : Action
{
    public float PatrolSpeed = 0.2f;
    public float PatrolAngularSpeed = 40f;
    public float PatrolAcceleration = 0.2f;
    public float PatrolStoppingDistance = 0f;

    public override void Act(StateController controller)
    {
         controller.agent.isStopped = false;
         controller.current_destination = controller.m_wayPoints[controller.TheNearestWayPoint()].position;
         controller.agent.destination = controller.m_wayPoints[controller.TheNearestWayPoint()].position;
         controller.chaseCounter = 0;
    }

    private void SetPatrolParameters(StateController controller)
    {
        // Define the Walking Parameters
        controller.agent.speed = PatrolSpeed;
        controller.agent.angularSpeed = PatrolAngularSpeed;
        controller.agent.acceleration = PatrolAcceleration;
        controller.agent.stoppingDistance = PatrolStoppingDistance;
    }
}
