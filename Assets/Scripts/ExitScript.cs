using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class ExitScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }

        if (CrossPlatformInputManager.GetButtonDown("ChangeScene"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
