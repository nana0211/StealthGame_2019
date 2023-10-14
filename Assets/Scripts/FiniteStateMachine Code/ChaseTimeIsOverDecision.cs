using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/ChaseTimeIsOverDecision")]
public class ChaseTimeIsOverDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        if(controller.chaseCounter < controller.chaseTime)
        {
            controller.chaseCounter += Time.deltaTime;
        }
        return controller.chaseCounter >= controller.chaseTime;
    }
}