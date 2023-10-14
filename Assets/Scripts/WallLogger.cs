using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLogger : MonoBehaviour
{

    [HideInInspector] public int EachFrameLog = 1;

    public GameObject m_WallTrigger;

    private FadeWallScript fadeWall;
    private LogManagerScript logManager;
    private int frameCounter;

    // Start is called before the first frame update
    void Start()
    {
        // Initializer
        fadeWall = m_WallTrigger.GetComponent<FadeWallScript>();
        logManager = GameObject.FindGameObjectWithTag("LogManager").GetComponent<LogManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCounter >= EachFrameLog)
        {
            logManager.LogWriter(WallUpdate());
        }
        else
        {
            frameCounter++;
        }
    }


    private string WallUpdate()
    {

        Vector3 pos = m_WallTrigger.transform.position;
        Vector3 rot = m_WallTrigger.transform.rotation.eulerAngles;

        string msg = "BRW;" + logManager.GetCurrentTimeStamp() + ";" + m_WallTrigger.name + ";" 
            + pos.x + "," + pos.y + "," + pos.z + ";" + rot.x + "," + rot.y + "," + rot.z + ";"
            + "BRW_TRG," + fadeWall.hasWallTriggered + ";" + "BRW_LYR," + m_WallTrigger.layer;

        return msg;
    }

    private void OnDestroy()
    {
        Vector3 pos = m_WallTrigger.transform.position;
        Vector3 rot = m_WallTrigger.transform.rotation.eulerAngles;

        string msg = "BRW_DTRY;" + logManager.GetCurrentTimeStamp() + ";" + gameObject.name + ";"
            + pos.x + "," + pos.y + "," + pos.z + ";" + rot.x + "," + rot.y + "," + rot.z + ";"
            + "BRW_TRG," + fadeWall.hasWallTriggered + ";" + "BRW_LYR," + m_WallTrigger.layer;

        logManager.LogWriter(msg);
    }
}
