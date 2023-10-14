using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelLogger : MonoBehaviour
{
    public GameObject m_EndLevelTrigger;

    [HideInInspector] public int EachFrameLog = 1;

    private LevelFinish finishTrigger;
    private LogManagerScript logManager;
    private int frameCounter;

    // Start is called before the first frame update
    void Start()
    {
        // Initializer
        finishTrigger = m_EndLevelTrigger.GetComponent<LevelFinish>();
        logManager = GameObject.FindGameObjectWithTag("LogManager").GetComponent<LogManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCounter >= EachFrameLog)
        {
            logManager.LogWriter(EndLevelUpdate());
        }
        else
        {
            frameCounter++;
        }
    }


    string EndLevelUpdate()
    {

        Vector3 pos = m_EndLevelTrigger.transform.position;
        Vector3 rot = m_EndLevelTrigger.transform.rotation.eulerAngles;


        string msg = "ELV;" + logManager.GetCurrentTimeStamp() + ";"
            + pos.x + "," + pos.y + "," + pos.z + ";" + rot.x + "," + rot.y + "," + rot.z + ";"
            + "PlayerFinish," + finishTrigger.playerHasFinished;

        return msg;
    }

    private void OnDestroy()
    {
        Vector3 pos = m_EndLevelTrigger.transform.position;
        Vector3 rot = m_EndLevelTrigger.transform.rotation.eulerAngles;


        string msg = "ELV;" + logManager.GetCurrentTimeStamp() + ";"
            + pos.x + "," + pos.y + "," + pos.z + ";" + rot.x + "," + rot.y + "," + rot.z + ";"
            + "PlayerFinish," + finishTrigger.playerHasFinished;


        logManager.LogWriter(msg);
    }
}
