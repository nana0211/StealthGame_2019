using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JSONLogger : MonoBehaviour
{

    public string LogLocation = "LogFolder/";
    public string LogFileName = "Logs";
    public bool IsLogging = true;

    private StreamWriter sw;


    void Awake()
    {
        LogFileName += "_" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".txt";
        string finalpath = Path.Combine(LogLocation, LogFileName);
        Directory.CreateDirectory(LogLocation);
        sw = new StreamWriter(finalpath); 
    }

    void FixedUpdate()
    {
        // Each Fixed Update we Save to File!
        if(IsLogging)
            SaveJSONToFile();
    }

    private void OnApplicationQuit()
    {
        sw.Close();
    }

    private void SaveJSONToFile()
    {
        List<LogInfo> jsonLog = new List<LogInfo>();
        LoggerGeneric[] logObjects = GameObject.FindObjectsOfType<LoggerGeneric>();

        foreach (LoggerGeneric lg in logObjects)
        {
            jsonLog.Add(lg.GetCurrentObjectInfo());
        }
        SaveToFile(JsonConvert.SerializeObject(new SceneObject(SceneManager.GetActiveScene().name, jsonLog.ToArray())));
    }

    private void SaveToFile(string JSONString)
    {
        // save to File Here
        sw.WriteLine(JSONString);
    }

    class SceneObject
    {
        public string SceneName;
        public DateTime timestamp;
        public LogInfo[] SceneComponents;

        public SceneObject(string SceneName, LogInfo[] jsonLog)
        {
            this.SceneName = SceneName;
            this.timestamp = DateTime.Now;
            this.SceneComponents = jsonLog;
        }

    }
}
