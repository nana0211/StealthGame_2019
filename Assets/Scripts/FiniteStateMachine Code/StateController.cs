using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;
using System;

public class StateController : MonoBehaviour
{

    [HideInInspector]public Rigidbody m_rigidbody;
    public float enemyAttackRate = 5f;
    public float searchingTurnSpeed = 120f;
    public float searchDuration = 10f;
    public FirstPersonControllerTank m_player; 
    public float idleTurnAngle = 180f;
    public float idleTotalTurnTime = 5f;
    public float idleTimeToTurn = 5f;
    [HideInInspector] public float waitTimePatrol = 3f;
    public Text m_freezeText;
    // sleeper timers
    public float waitTimeSleeper = 3f;
    public float sleepSeconds = 6f; //This is duration from to sleep -> the next to sleep. 
    [HideInInspector] public float currentSleepCounter = 0f; //changes
    [HideInInspector] public float firstWaitCounter = 0f;

    // freeze time
    public float freezeTime = 2f;
    public static float immunityTime = 3f;
    public static bool setImmTime = false;
   
    public EnemyTypesEnum.EnemyType controllerType;
    public State currState;
    public VisionCone npcVision;
    public State remainState; // If nothing has changed, just remain in the same state.
    private bool isForward = false;
    private bool isBackward = false;
    public int m_AgentID;
    public Transform[] m_wayPoints;
    public int currWaypoint = -1;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public List<GameObject> waypoint_list;
    [HideInInspector] public Vector3 current_destination;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed = 0f;
    [HideInInspector] public bool playerHasBeenCatched = false;
    [HideInInspector] public bool actionHasBeenTaken = false;

    [HideInInspector] public bool idleIsTurning = false;
    [HideInInspector] public bool isSleeping = false;
    [HideInInspector] public Vector3 originalPosition;
    [HideInInspector] public Quaternion originalRotation;
    [HideInInspector] public Vector3 lastSeenPosition;
    [HideInInspector] public bool firstSleep = false;
    [HideInInspector] public float timeUntilNextTurn = 0f;
    [HideInInspector] public float currentTimeTurn = 0f;
    [HideInInspector] public Quaternion currentTurn = Quaternion.Euler(0f, 0f, 0f);
    [HideInInspector] public float PatrolWaitAtWaypoint = 4.0f;

    //ChaseTimer;
    [HideInInspector] public float chaseCounter = 0f;
    [HideInInspector] public float chaseTime = 5f;
    //SelfFreezeTimer:
    [HideInInspector] public float selfFreezeCounter = 0f;
    [HideInInspector] public float selfFreezeTime = 4f;
    // Start is called before the first frame update
    void Awake()
    { 
        if ((controllerType != EnemyTypesEnum.EnemyType.PATROL_FREEZER || controllerType != EnemyTypesEnum.EnemyType.PATROL_KILLER) && m_wayPoints == null)
        {
            Debug.Log("No Waypoint List as Child. This is an Error!");
        }
        originalPosition = transform.position;
        originalRotation = transform.localRotation;
        
        current_destination = Vector3.zero;
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        m_rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        print(currState);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        currState.UpdateState(this);
        m_freezeText.text = "selfFreezeTime"+selfFreezeCounter.ToString()+ "\n"+ "extendedChaseTime" + chaseCounter.ToString();
    }

    private void OnDrawGizmos()
    {
        if(currState != null && npcVision != null)
        {
            Gizmos.color = currState.sceneGizmoColor;
        }
    }

    public Vector3 PickFollowingWaypoint()
    {
        if (m_wayPoints.Length == 0)
        {
            return Vector3.zero;
        }

        if (currWaypoint == -1)
        {
            isForward = true; 
        }
        
        if (isForward)
        {
            if (currWaypoint >= m_wayPoints.Length - 1)
            {
                isForward = false;
                currWaypoint = currWaypoint - 1;
            }else
            {
                currWaypoint++;
            }
        }
        else
        {
            if (currWaypoint > 0)
            {
                currWaypoint--;
            }else
            {
                isForward = true;
                currWaypoint++;
            }
            
        }
      
        return m_wayPoints[currWaypoint].position;
    }
    public int TheNearestWayPoint ()
    {
        float[] dists = new float[m_wayPoints.Length];
        float minDistance = 0.0f;
        for (int i = 0; i<m_wayPoints.Length; i++)
        {
            dists[i] = Vector3.Distance(m_wayPoints[i].position, transform.position);

        }
        minDistance = dists[0];
        for (int i = 1; i < m_wayPoints.Length; i++)
        {
            minDistance = Mathf.Min(dists[i], minDistance);
        }
        //print(Array.IndexOf(dists, minDistance));
        return currWaypoint = Array.IndexOf(dists, minDistance);
    }
    public void TransitionToState(State nextState)
    {
        if(nextState != remainState)
        {
            currState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }

    private GameObject GetChildWithTag(string tag)
    {
        int childCount = this.transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            if (this.transform.GetChild(i).tag == tag)
            {
                return this.transform.GetChild(i).gameObject;
            }
        }
        return null;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("This is executed 111");
        if (other.tag == "Player" && !isSleeping && !other.transform.GetComponent<FirstPersonControllerTank>().IsInvisible)
        {
            Debug.Log("This is executed 222");
            playerHasBeenCatched = true;

            if (this.tag == "Freezer" && !setImmTime)
            {
                immunityTime = freezeTime + immunityTime;
                setImmTime = true;
            }
            else if (!setImmTime)
            {
                setImmTime = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !isSleeping && !other.transform.GetComponent<FirstPersonControllerTank>().IsInvisible)
        {
            Debug.Log("This is executed 222");
            playerHasBeenCatched = true;

            if (this.tag == "Freezer" && !setImmTime)
            {
                immunityTime = freezeTime + immunityTime;
                setImmTime = true;
            }
            else if (!setImmTime)
            {
                setImmTime = true;
            }
        }
    }

}
