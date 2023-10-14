using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableAI/Decisions/ArrivedDestination")]
public class ArrivedDestinationDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return IsEnemyArrived(controller);
    }

    private bool IsEnemyArrived(StateController controller)
    {
        bool isReturned = false; 
        if (!controller.agent.pathPending)
        {
            if (controller.agent.remainingDistance <= controller.agent.stoppingDistance)
            {
                if (!controller.agent.hasPath || controller.agent.velocity.sqrMagnitude == 0f)
                {

                    isReturned = true;
                }
                
            }
            
        }
        return isReturned;
    }
}
