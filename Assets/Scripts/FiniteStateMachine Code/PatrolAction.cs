using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public float PatrolSpeed = 0.2f;
    public float PatrolAngularSpeed = 40f;
    public float PatrolAcceleration = 0.2f;
    public float PatrolStoppingDistance = 0.4f;

    public override void Act(StateController controller)
    {
        Patrol(controller);
    }


    private void Patrol(StateController controller)
    {

        SetPatrolParameters(controller);
        controller.waitTimePatrol -= Time.deltaTime;

        if (controller.current_destination != Vector3.zero)
        {
            controller.agent.destination = controller.current_destination;
            controller.agent.isStopped = false; // Resume to destination
  

            // Have we arrived at our destination?
            if (controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending)
            {
                if (controller.waitTimePatrol <= 0f)
                {    // wait then pick the next waypoint
                    controller.current_destination = controller.PickFollowingWaypoint();
                    controller.waitTimePatrol = controller.PatrolWaitAtWaypoint;
                }

            }
        }
        else
        {       //from initial position at the beginning of the game
                controller.current_destination = controller.PickFollowingWaypoint();
                controller.waitTimePatrol =controller.PatrolWaitAtWaypoint;
         }

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
