using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableAI/Actions/SelfFreeze")]
public class SelfFreezeAction : Action
{
    public override void Act(StateController controller)
    {
        controller.agent.isStopped = true;
        controller.isSleeping = true;
        controller.m_rigidbody.isKinematic = true;
        controller.m_player.transform.gameObject.layer = 2;
        controller.playerHasBeenCatched = false;
        controller.actionHasBeenTaken = false;
    }
}
