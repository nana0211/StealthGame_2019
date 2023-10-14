using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PluggableAI/Actions/GoToLastSeen")]
public class GoToLastSeen : Action
{
    private const float speed = 3f;
    private const float angularSpeed = 220f;
    public override void Act (StateController controller)
    {
        GoToLast(controller);
    }

    private void GoToLast(StateController controller)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(controller.lastSeenPosition, out hit, 1.0f, -1))
        {
            SetBackParameters(controller);
            controller.lastSeenPosition = hit.position;
            controller.agent.destination = controller.lastSeenPosition;
            Debug.Log("go with the samplePosition");
            controller.agent.isStopped = false;
           
        }
    }


    private void SetBackParameters(StateController controller)
    {
        // Define Chase Speed Parameters
        controller.agent.speed = speed;
        controller.agent.angularSpeed = angularSpeed;
        controller.agent.stoppingDistance = 0.0f;
    }
    
}
