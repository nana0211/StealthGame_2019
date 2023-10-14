using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Sleep")]
public class SleepAction : Action
{
    //private const float fadeOutTime = 1f;
    public override void Act(StateController controller)
    {
        if (controller.firstSleep == false)
        {
            //////wait before first function call
            if (controller.waitTimeSleeper <= controller.firstWaitCounter)
            {
                Sleep(controller);
                controller.firstSleep = true;
            }
            else
            {
                controller.firstWaitCounter += Time.deltaTime;
            }
        }else
        {
            Sleep(controller);
        }
    }
    public void Sleep(StateController controller)
    {
        // Check if the NPC has sleeped enough
        if(controller.currentSleepCounter >= controller.sleepSeconds)
        {
            SleepOrWake(controller, controller.isSleeping);
            controller.currentSleepCounter = 0f;
        }
        else
        {
            controller.currentSleepCounter += Time.deltaTime;
        }
    }

    public void SleepOrWake(StateController controller, bool isSleeping)
    {
        controller.isSleeping = !isSleeping;
        Color newCol = controller.npcVision.GetMeshRenderer().material.color;
        if (!isSleeping)
            newCol.a = 0;
        else
            newCol.a = 1;
        controller.npcVision.GetMeshRenderer().material.color = newCol;
    }
}


