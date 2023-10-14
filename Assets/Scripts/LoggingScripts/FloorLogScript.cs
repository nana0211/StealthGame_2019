using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLogScript : LoggerGeneric
{

    public MovingFloorController moveFloor;

    // Start is called before the first frame update
    void Start()
    {
        moveFloor = GetComponent<MovingFloorController>();
    }

    public override LogInfo GetCurrentObjectInfo()
    {
        string name = gameObject.name;
        string tag = gameObject.tag;

        Vector3 pos = gameObject.transform.position;
        Vector3 rot = gameObject.transform.rotation.eulerAngles;
        Vector3 scale = gameObject.transform.localScale;

        FloorInfo info = new FloorInfo(name, tag, pos, rot, scale, moveFloor);

        return info;
    }


    class FloorInfo: LogInfo
    {
        public string name;
        public string tag;

        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public MoveFloor MoveFloor;

        public FloorInfo(string name, string tag, Vector3 position, Vector3 rotation, Vector3 scale, MovingFloorController moveFloor)
        {
            this.name = name;
            this.tag = tag;

            this.position = position;
            this.rotation = rotation;
            this.scale = scale;

            this.MoveFloor = new MoveFloor(moveFloor);
        }
    }


    class MoveFloor
    {
        public float MovingSpeed;
        public float Acceleration;

        public Vector3[] WaypointPosition;
        public Vector3[] WaypointRotation;
        public Vector3[] WaypointScale;

        public int CurrentWaypointIndex;
        public Vector3 CurrentDestination;
        public bool PlayerIsOnTrigger;

        public MoveFloor(MovingFloorController movefloor)
        {
            this.MovingSpeed = movefloor.movingSpeed;
            this.Acceleration = movefloor.acceleration;

            WaypointPosition = new Vector3[movefloor.wayPoints.Length];
            WaypointRotation = new Vector3[movefloor.wayPoints.Length];
            WaypointScale = new Vector3[movefloor.wayPoints.Length];
            for (int i = 0; i < movefloor.wayPoints.Length; i++)
            {
                WaypointPosition[i] = movefloor.wayPoints[i].position;
                WaypointRotation[i] = movefloor.wayPoints[i].rotation.eulerAngles;
                WaypointScale[i] = movefloor.wayPoints[i].localScale;
            }

            this.CurrentWaypointIndex = movefloor.currentWayPointIndex;
            this.CurrentDestination = movefloor.currentDestination;
            this.PlayerIsOnTrigger = movefloor.playerIsOnTrigger;
        }
    }
}
