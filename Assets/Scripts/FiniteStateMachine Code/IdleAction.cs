using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Idle")]
public class IdleAction : Action
{
  

    public override void Act(StateController controller)
    {
        TurnAround(controller);
    }

    private void TurnAround(StateController controller)
    {
        if(controller.timeUntilNextTurn > controller.idleTimeToTurn)
        {
            if (controller.idleIsTurning)
            {
                // Turn
                if (controller.currentTimeTurn < 1)
                {
                    controller.currentTimeTurn += Time.deltaTime / controller.idleTotalTurnTime;
                    controller.gameObject.transform.rotation = Quaternion.Lerp(controller.gameObject.transform.rotation, controller.currentTurn, controller.currentTimeTurn);
                }
                else
                {
                    controller.idleIsTurning = false;
                    controller.timeUntilNextTurn = 0f;
                    controller.currentTimeTurn = 0f;
                }
            }
            else
            {
                controller.currentTurn = Quaternion.Euler(controller.gameObject.transform.rotation.eulerAngles + new Vector3(0f,-controller.idleTurnAngle,0f));
                controller.idleIsTurning = true;
            }
        }
        else
        {
            controller.timeUntilNextTurn += Time.deltaTime;
        }

        
    }

}
