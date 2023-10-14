using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 0.2f;

    private Transform player;
    private float vertInput;
    private float horizInput;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        horizInput = CrossPlatformInputManager.GetAxis("Horizontal") * playerSpeed;
        vertInput = CrossPlatformInputManager.GetAxis("Vertical") * playerSpeed;
        player.Translate(new Vector3(horizInput, 0, vertInput));

        // Quit Game
        if (CrossPlatformInputManager.GetButton("Cancel"))
        {
            Application.Quit();
        }
    }
}
