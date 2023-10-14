using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableAI/Decisions/SelfFreezeIsOverDecision")]
public class SelfFreezeIsOverDecision : Decision
{
    // Start is called before the first frame update
    public override bool Decide(StateController controller)
    {
        if (controller.selfFreezeCounter < controller.selfFreezeTime)
        {
            controller.selfFreezeCounter += Time.deltaTime;
        }
        return controller.selfFreezeCounter >= controller.selfFreezeTime;
    }
}
