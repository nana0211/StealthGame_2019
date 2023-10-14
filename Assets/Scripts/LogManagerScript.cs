using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading;
using System.Collections.Concurrent;

public class LogManagerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public int EachFrameLog = 1;
    private string LogPath = @"/Log/";
    public string FileName = "Stuff";

    private PlayerLogger pLog;
    private List<EnemyLogger> eLog;
    private CollectionLogger cLog;
    private List<WallLogger> wLog;

    public bool m_TurnOnLogging = true;


    /// <summary>
    /// We use concurrent queue to create a thread safe log system. 
    /// This way we make sure that the file is being effectively written to a file without losing data.
    /// </summary>
    private ConcurrentQueue<string> buffer = new ConcurrentQueue<string>(); 
    private Thread bufferWriter;
    private volatile bool TimeToQuit = false;

    void Awake()
    {
        LogPath = Application.dataPath + LogPath;
        Directory.CreateDirectory(Path.GetDirectoryName(LogPath));

        LogPath += FileName + "_" + CreateFileName() + ".txt";
        Debug.Log(LogPath);

        // Get All Loggers
        // Player
        pLog = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLogger>();
        
        // Enemies
        eLog = new List<EnemyLogger>();
        GameObject[] aux = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in aux)
        {
            eLog.Add(enemy.GetComponent<EnemyLogger>());
        }

        // Collectible Manager
        cLog = GameObject.FindGameObjectWithTag("CollectionManager").GetComponent<CollectionLogger>();

        wLog = new List<WallLogger>();
        aux = GameObject.FindGameObjectsWithTag("BreakWall");
        foreach (GameObject wall in aux)
        {
            wLog.Add(wall.GetComponent<WallLogger>());
        }


        ChangeLogFrameRate();

        // Start the Thread
        bufferWriter = new Thread(new ThreadStart(BufferWriter));
        bufferWriter.Start();
    }

    private void OnDestroy()
    {
        TimeToQuit = true;
        while (bufferWriter.IsAlive)
        {
            Debug.Log("Waiting for Logging Thread to Terminate");
        }
    }

    public void LogWriter(string playerLog)
    {
        if(m_TurnOnLogging)
            buffer.Enqueue(playerLog);
    }

    public float GetCurrentTimeStamp()
    {
        return Time.time;
    }

    private string CreateFileName()
    {
        return "" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "_" 
            + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second;
    }

    private void ChangeLogFrameRate()
    {
        pLog.EachFrameLog = this.EachFrameLog;

        foreach (EnemyLogger e in eLog)
        {
            e.EachFrameLog = this.EachFrameLog;
        }

        cLog.EachFrameLog = this.EachFrameLog;

        foreach(WallLogger w in wLog)
        {
            w.EachFrameLog = this.EachFrameLog;
        }

    }

    // For log Writing it is necessary to constantly be writing to a file without interrupting the game, thus we will create our own thread for this purpose.
    private void BufferWriter()
    {
        while (!TimeToQuit)
        {
            while (!buffer.IsEmpty)
            {
                if (buffer.Count > 0 && buffer.TryDequeue(out string toWrite))
                {
                    File.AppendAllText(LogPath, toWrite + "\n");
                }
            }
        }
    }
}
