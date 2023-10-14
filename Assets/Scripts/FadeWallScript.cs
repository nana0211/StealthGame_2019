using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeWallScript : MonoBehaviour
{

    public float m_TimeToFade = 8f;
    public float m_AlphaToSeeThrough = 0.4f; // How much alpha necessary for the enemy to see through this wall
    public float m_AlphaToDestroy = 0.1f; // How much alpha necessary to finally destroy the wall object
    public GameObject WallObject;
    public Material toFadeMaterial;

    private float currentTime = 0f; // Keep track of the current time
    private MeshRenderer meshRender;
    private Color colorFade;

    [HideInInspector] public bool hasWallTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        hasWallTriggered = false;
        currentTime = 0f;
        meshRender = WallObject.GetComponent<MeshRenderer>();

        colorFade = meshRender.material.color;
        colorFade.a = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (hasWallTriggered)
        {
            FadeWall();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // Apply the Fade Function
        if(other.tag == "Player")
        {
            hasWallTriggered = true;
            meshRender.material = toFadeMaterial;
        }
    }

    private void FadeWall()
    {
        currentTime += Time.deltaTime;
        meshRender.material.color = Color.Lerp(meshRender.material.color, colorFade, currentTime / m_TimeToFade);

        if(meshRender.material.color.a <= m_AlphaToSeeThrough)
        {
            WallObject.layer = 0;
        }

        if(meshRender.material.color.a <= m_AlphaToDestroy)
        {
            Destroy(WallObject);
        }

    }

}
