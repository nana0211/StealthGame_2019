using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/NPCFade")]
public class FadeNPCAction : Action
{

    private Color alphaColor;
    public float TimeToFade = 0.2f;
    public float AlphaToDestroy = 0.1f;

    public override void Act(StateController controller)
    {
        Fade(controller);
    }

    public void Fade(StateController controller)
    {
        if (controller.playerHasBeenCatched)
        {
            controller.gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(
                controller.gameObject.GetComponent<MeshRenderer>().material.color, Color.clear, TimeToFade * Time.deltaTime);
        }

        // If the NPC has fully faded, destroy it.
        if (controller.gameObject.GetComponent<MeshRenderer>().material.color.a <= AlphaToDestroy)
        {
            Destroy(controller.gameObject);
        }
    }

}
