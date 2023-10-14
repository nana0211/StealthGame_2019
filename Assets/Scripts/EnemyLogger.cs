using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogger : MonoBehaviour
{
    [HideInInspector] public int EachFrameLog = 1;

    private StateController controller;
    private LogManagerScript logManager;
    private int frameCounter;

    private void Start()
    {
        // Initializer
        controller = GetComponent<StateController>();
        logManager = GameObject.FindGameObjectWithTag("LogManager").GetComponent<LogManagerScript>();
    }

    private void FixedUpdate()
    {
        if (frameCounter >= EachFrameLog)
        {
            logManager.LogWriter(GetEnemyStats());
        }
        else
        {
            frameCounter++;
        }
    }


    private string GetEnemyStats()
    {
        Vector3 pos = controller.gameObject.transform.position;
        Vector3 rot = controller.gameObject.transform.rotation.eulerAngles;

        string logmsg = "ENY" + ";" + logManager.GetCurrentTimeStamp() + ";" + controller.m_AgentID +";" + controller.controllerType + ";" 
            + pos.x + "," + pos.y + "," + pos.z + ";" + rot.x + "," + rot.y + "," + rot.z + ";" + 
            "PLY_CTH," + controller.playerHasBeenCatched + ";" + "ACT_TKN," + controller.actionHasBeenTaken;

        return logmsg;
    }

    private void OnDestroy()
    {
        Vector3 pos = controller.gameObject.transform.position;
        Vector3 rot = controller.gameObject.transform.rotation.eulerAngles;
        string logmsg = "ENY_DTRY" + ";" + logManager.GetCurrentTimeStamp() + ";" + controller.m_AgentID+";" + controller.controllerType + ";"
            + pos.x + "," + pos.y + "," + pos.z + ";" + rot.x + "," + rot.y + "," + rot.z + ";" +
            "PLY_CTH," + controller.playerHasBeenCatched + ";" + "ACT_TKN," + controller.actionHasBeenTaken;

        logManager.LogWriter(logmsg);
    }
}
