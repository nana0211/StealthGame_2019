using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableAI/Actions/Scan")]
public class ScanAction : Action
{
    public override void Act(StateController controller)
    {
        Scan(controller);
    }

    private void Scan(StateController controller)
    {
        controller.agent.isStopped = true;
        if (!controller.CheckIfCountDownElapsed(controller.searchDuration))
        {
            controller.transform.Rotate(0, controller.searchingTurnSpeed * Time.deltaTime, 0);
        }
        
    }
}
