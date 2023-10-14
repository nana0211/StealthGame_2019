using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLogger : MonoBehaviour
{

    [HideInInspector] public int EachFrameLog = 1;

    private FirstPersonControllerTank controller;
    private LogManagerScript logManager;
    private int frameCounter;

    // Start is called before the first frame update
    void Start()
    {
        // Initializer
        controller = GetComponent<FirstPersonControllerTank>();
        logManager = GameObject.FindGameObjectWithTag("LogManager").GetComponent<LogManagerScript>();
        StartLog();
    }

    private void FixedUpdate()
    {
        if(frameCounter >= EachFrameLog)
        {
            logManager.LogWriter(GetPlayerStats());
        }
        else
        {
            frameCounter++;
        }
    }

    private string GetPlayerStats()
    {
        Vector3 pos = controller.gameObject.transform.position;
        Vector3 rot = controller.gameObject.transform.rotation.eulerAngles;

        string logmsg = "PLY" + ";" + logManager.GetCurrentTimeStamp() + ";" 
            + pos.x + "," + pos.y + "," + pos.z + ";" + rot.x + "," + rot.y + "," + rot.z + ";" 
            + controller.m_timeLeft + ";" + controller.m_woundNumber + ";" + controller.m_disableControlTimer
            + controller.m_walkingSpeed + ";" + controller.m_rotateSpeed;


        return logmsg;

    }

    private void StartLog()
    {
        logManager.LogWriter("STR;" + logManager.GetCurrentTimeStamp() + ";" + SceneManager.GetActiveScene().name);
    }

}
