using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/SleepVariant")]
public class SleepVariantAction :Action
{
 
    private float transitionTime;
    // Start is called before the first frame update
    public override void Act(StateController controller)
    {
        if (controller.firstSleep == false)
        {
            //////wait before first function call
            if (controller.waitTimeSleeper <= controller.firstWaitCounter)
            {
                ToSleep(controller);
                controller.firstSleep = true;
            }
            else
            {
                controller.firstWaitCounter += Time.deltaTime;
            }
        }
        else
        {
            ToSleep(controller);
        }
    }
    public void ToSleep(StateController controller)
    {
        controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, controller.originalRotation, .8f * Time.deltaTime);

        // restart timer after every sleep cycle
        if (controller.currentSleepCounter >= controller.sleepSeconds)
        {
            controller.currentSleepCounter = 0f;
        }
        else
        {
            controller.currentSleepCounter += Time.deltaTime;
        }

        // turn vision cone on or off at half sleep cycle
        float alpha_value = 1.0f;
        if (controller.currentSleepCounter <= controller.sleepSeconds / 2)
        {
            controller.isSleeping = true;
            alpha_value = 0f;
        }
        else 
        {
            controller.isSleeping = false;
            alpha_value = 1.0f;
        }

        //sDebug.Log("output alpha" + alpha_value);
        Color newCol = controller.npcVision.GetMeshRenderer().material.color;
        newCol.a = alpha_value;
        controller.npcVision.GetMeshRenderer().material.color = newCol;
    }
}
