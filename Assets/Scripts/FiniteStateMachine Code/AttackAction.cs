using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action
{

    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        if (controller.npcVision.playerTarget != null)
        {
            if (controller.playerHasBeenCatched && !controller.actionHasBeenTaken)
            {
                controller.npcVision.playerTarget.GetComponent<FirstPersonControllerTank>().IncWoundCounter(1);
                controller.actionHasBeenTaken = true;
                controller.npcVision.playerTarget.GetComponent<FirstPersonControllerTank>().isGetAttacked = controller.npcVision.playerTarget.GetComponent<FirstPersonControllerTank>().IsInvisible ? false : true;
                
            }
        }
    }
}
