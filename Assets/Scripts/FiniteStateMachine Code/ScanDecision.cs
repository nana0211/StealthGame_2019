using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Scan")]
public class ScanDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool scanComplete = Scan(controller);
        return scanComplete;
    }

    private bool Scan(StateController controller)
    {
        return controller.CheckIfCountDownElapsed(controller.searchDuration);
    }
}
