using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionLogger : MonoBehaviour
{

    [HideInInspector] public int EachFrameLog = 1;

    private CollectionManager collectionManager;
    private LogManagerScript logManager;
    private int frameCounter;

    // Start is called before the first frame update
    void Start()
    {
        // Initializer
        collectionManager = GetComponent<CollectionManager>();
        logManager = GameObject.FindGameObjectWithTag("LogManager").GetComponent<LogManagerScript>();
    }

    private void FixedUpdate()
    {
        if (frameCounter >= EachFrameLog)
        {
            logManager.LogWriter(GetCollectionStats());
        }
        else
        {
            frameCounter++;
        }
    }

    private string GetCollectionStats()
    {
        string str = "COL;" + logManager.GetCurrentTimeStamp() + ";" + collectionManager.CollectiblesLeft();
        foreach(string s in collectionManager.GetAllCollectibleID())
        {
            str += ";" + s;
        }
        return str;
    }
}
