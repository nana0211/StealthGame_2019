using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Catch")]
public class CatchDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool isCatched = IsPlayerWithinRange(controller);
        return isCatched;
    }

    private bool IsPlayerWithinRange(StateController controller)
    {
        Debug.Log(controller.playerHasBeenCatched);
        return controller.playerHasBeenCatched;
    }
}
