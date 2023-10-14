using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {
        if(controller.npcVision.playerTarget != null)
        {
            //Debug.Log(controller.m_AgentID + "I have a target");
            controller.lastSeenPosition = Vector3.zero;
            controller.chaseTarget = controller.npcVision.playerTarget;
            return true;
        }
        else
        {
          
            if (controller.chaseTarget != null)
            {
                controller.lastSeenPosition = controller.chaseTarget.position; 
            }
            controller.chaseTarget = null;
            return false;
        }
    }
}
