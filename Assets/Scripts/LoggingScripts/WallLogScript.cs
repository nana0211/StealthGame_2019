using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLogScript : LoggerGeneric
{

    private MovingWallController moveWall;

    // Start is called before the first frame update
    void Start()
    {
        moveWall = GetComponent<MovingWallController>();
    }

    public override LogInfo GetCurrentObjectInfo()
    {
        string name = gameObject.name;
        string tag = gameObject.tag;

        Vector3 pos = gameObject.transform.position;
        Vector3 rot = gameObject.transform.rotation.eulerAngles;
        Vector3 scale = gameObject.transform.localScale;

        WallInfo info =  new WallInfo(name, tag, pos, rot, scale, moveWall);

        return info;
    }


    class WallInfo : LogInfo
    {
        public string name;
        public string tag;

        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public MovingWallInfo wallControl;

        public WallInfo(string name, string tag, Vector3 position, Vector3 rotation, Vector3 scale, MovingWallController wallControl)
        {
            this.name = name;
            this.tag = tag;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;

            this.wallControl = new MovingWallInfo(wallControl);
        }
    }

    [Serializable]
    class MovingWallInfo
    {
        public float movingSpeed;
        public float acceleration;
        public Vector3[] WaypointPosition;
        public Vector3[] WaypointRotation;
        public Vector3[] WaypointScale;
        public int currentWaypointIndex;
        public Vector3 currentDestination;
        public bool PlayerIsOnTrigger;

        public MovingWallInfo(MovingWallController control)
        {
            this.movingSpeed = control.movingSpeed;
            this.acceleration = control.acceleration;

            WaypointPosition = new Vector3[control.wayPoints.Length];
            WaypointRotation = new Vector3[control.wayPoints.Length];
            WaypointScale = new Vector3[control.wayPoints.Length];
            for (int i = 0; i < control.wayPoints.Length; i++)
            {
                WaypointPosition[i] = control.wayPoints[i].position;
                WaypointRotation[i] = control.wayPoints[i].rotation.eulerAngles;
                WaypointScale[i] = control.wayPoints[i].localScale;
            }

            this.currentWaypointIndex = control.currentWayPointIndex;
            this.currentDestination = control.currentDestination;
            this.PlayerIsOnTrigger = control.playerIsOnTrigger;
        }
    }
}
