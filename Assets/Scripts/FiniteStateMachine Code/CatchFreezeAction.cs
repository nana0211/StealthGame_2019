using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Freeze")]
public class CatchFreezeAction : Action
{

    public override void Act(StateController controller)
    {
        Freeze(controller);
    }

    private void Freeze(StateController controller)
    {
        if (controller.npcVision.playerTarget != null)
        {
            if (controller.playerHasBeenCatched && !controller.actionHasBeenTaken)
            {
                controller.npcVision.playerTarget.GetComponent<FirstPersonControllerTank>().DisableControls(controller.freezeTime);
                controller.actionHasBeenTaken = true;
                controller.npcVision.playerTarget.GetComponent<FirstPersonControllerTank>().isGetAttacked =
                controller.npcVision.playerTarget.GetComponent<FirstPersonControllerTank>().IsInvisible ? false : true;
            }
        }
    }
    
}
