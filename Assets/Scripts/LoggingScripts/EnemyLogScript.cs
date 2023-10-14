using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogScript : LoggerGeneric
{

    private VisionCone visionCone;
    private StateController controller;

    // Start is called before the first frame update
    void Start()
    {
        visionCone = GetComponent<VisionCone>();
        controller = GetComponent<StateController>();
    }

    public override LogInfo GetCurrentObjectInfo()
    {
        string name = gameObject.name;
        string tag = gameObject.tag;

        Vector3 pos = gameObject.transform.position;
        Vector3 rot = gameObject.transform.rotation.eulerAngles;
        Vector3 scale = gameObject.transform.localScale;

        EnemyInfo info = new EnemyInfo(name, tag, pos, rot, scale, visionCone, controller);

        return info;
    }

    class EnemyInfo : LogInfo
    {
        public string name;
        public string tag;

        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public EnemyCone VisionCone;
        public EnemyState EnemyState;


        public EnemyInfo(string name, string tag, Vector3 position, Vector3 rotation, Vector3 scale, VisionCone cone, StateController stateControl)
        {
            this.name = name;
            this.tag = tag;

            this.position = position;
            this.rotation = rotation;
            this.scale = scale;

            this.VisionCone = new EnemyCone(cone);
            this.EnemyState = new EnemyState(stateControl);
        }
    }

    [Serializable]
    class EnemyState
    {
        public float enemyAttackRate;
        public float searchingTurnSpeed;
        public float searchDuration;
        public float idleTurnAngle;
        public float idleTotalTurnTime;
        public float idleTimeToTurn;
        public float waitTimePatrol;
        public float waitTimeSleeper;
        public float sleepSeconds;
        public float currentSleepCounter;
        public float firstWaitCounter;

        public float freezeTime;

        public EnemyTypesEnum.EnemyType controllerType;
        
        public string currState;
        public string remainState;
        
        public int m_AgentID;
        public Transform[] m_wayPoints;
        public int currWaypoint;

        public Vector3[] waypoint_list;
        public Vector3 current_destination;
        public Vector3 chaseTarget;
        public float stateTimeElapsed;
        public bool playerHasBeenCatched;
        public bool actionHasBeenTaken;

        public bool idleIsTurning;
        public bool isSleeping;
        public Vector3 originalPosition;
        public Vector3 originalRotation;
        public Vector3 lastSeenPosition;
        public bool firstSleep;
        public float timeUntilNextTurn;
        public float currentTimeTurn;
        public float PatrolWaitAtWaypoint;

        
        public float chaseCounter;
        public float chaseTime;
        
        public float selfFreezeCounter;
        public float selfFreezeTime;


        public EnemyState(StateController state)
        {
            this.enemyAttackRate = state.enemyAttackRate;
            this.searchingTurnSpeed = state.searchingTurnSpeed;
            this.searchDuration = state.searchDuration;
            this.idleTurnAngle = state.idleTurnAngle;
            this.idleTotalTurnTime = state.idleTotalTurnTime;
            this.idleTimeToTurn = state.idleTimeToTurn;
            this.waitTimePatrol = state.waitTimePatrol;
            this.waitTimeSleeper = state.waitTimeSleeper;
            this.sleepSeconds = state.sleepSeconds;
            this.currentSleepCounter = state.currentSleepCounter;
            this.firstWaitCounter = state.firstWaitCounter;

            this.freezeTime = state.freezeTime;
            this.controllerType = state.controllerType;

            this.currState = state.currState.name;
            this.remainState = state.remainState.name;

            this.m_AgentID = state.m_AgentID;
            this.m_wayPoints = state.m_wayPoints;
            this.currWaypoint = state.currWaypoint;

            List<Vector3> aux = new List<Vector3>();
            foreach(GameObject obj in state.waypoint_list)
            {
                aux.Add(obj.transform.position);
            }
            this.waypoint_list = aux.ToArray();

            this.current_destination = state.current_destination;

            if (state.chaseTarget != null)
                this.chaseTarget = state.chaseTarget.position;
            else
                this.chaseTarget = Vector3.zero;

            this.stateTimeElapsed = state.stateTimeElapsed;
            this.playerHasBeenCatched = state.playerHasBeenCatched;
            this.actionHasBeenTaken = state.actionHasBeenTaken;

            this.idleIsTurning = state.idleIsTurning;
            this.isSleeping = state.isSleeping;
            this.originalPosition = state.originalPosition;
            this.originalRotation = state.originalRotation.eulerAngles;
            this.lastSeenPosition = state.lastSeenPosition;
            this.firstSleep = state.firstSleep;
            this.timeUntilNextTurn = state.timeUntilNextTurn;
            this.currentTimeTurn = state.currentTimeTurn;
            this.PatrolWaitAtWaypoint = state.PatrolWaitAtWaypoint;

            this.chaseCounter = state.chaseCounter;
            this.chaseTime = state.chaseTime;

            this.selfFreezeCounter = state.selfFreezeCounter;
            this.selfFreezeTime = state.selfFreezeTime;
        }
    }

    [Serializable]
    class EnemyCone
    {
        public float viewRadius;
        public float viewAngle;
        public float meshResolution;

        public EnemyCone(VisionCone cone)
        {
            this.viewRadius = cone.viewRadius;
            this.viewAngle = cone.viewAngle;
            this.meshResolution = cone.meshResolution;
        }
    }
}
