using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableAI/Actions/BackToOriginal")]
public class BackToOriginalAction : Action
{
    private const float speed = 3f;
    private const float angularSpeed = 220f;
    public override void Act(StateController controller)
    {
        controller.chaseCounter = 0f;
        controller.selfFreezeCounter = 0f;
        Back(controller);
    }

    private void Back(StateController controller)
    {
        SetBackParameters(controller);
        controller.agent.destination = controller.originalPosition;
        controller.agent.isStopped = false;
        controller.playerHasBeenCatched = false;
    }

   
    private void SetBackParameters(StateController controller)
    {
        // Define Chase Speed Parameters
        controller.agent.speed = speed;
        controller.agent.angularSpeed = angularSpeed;
        controller.agent.stoppingDistance = 0f;
        
    }
}
