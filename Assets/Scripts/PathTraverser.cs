using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathTraverser : MonoBehaviour
{

    public GameObject waypoints;

    public float minWaitTime;
    public float maxWaitTime;

    private NavMeshAgent agent;
    private Vector3 curr_destination;
    private List<GameObject> waypoint_list;


    private bool IsWalking = false;
    private bool IsWaiting = false;

    private float CurrentWaitTimer = 0f;
    private float MaxWaitTimer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        waypoint_list = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;
        getWaypoints(waypoints);
        curr_destination = Vector3.zero;

        // Initialize a Destination
        SetNewRandomDestination();

    }

    // Update is called once per frame
    void Update()
    {
        // If Walking and getting near objective
        if(IsWalking && agent.remainingDistance <= 0.4f)
        {
            // Stop Walking
            IsWalking = false;

            // Waiting Period -- Set Timer to 0 and get a random time between the set time intervals
            IsWaiting = true;
            CurrentWaitTimer = 0f;
            MaxWaitTimer = GetRandomWaitTime();
        }

        // If Waiting
        if (IsWaiting)
        {
            CurrentWaitTimer += Time.deltaTime;
            if(CurrentWaitTimer >= MaxWaitTimer)
            {
                IsWaiting = false;
                SetNewRandomDestination();
            }
        }
    }

    void getWaypoints(GameObject waypoints)
    {
        foreach (Transform child in waypoints.GetComponentsInChildren<Transform>())
        {
            if(child.gameObject.tag == "waypoint")
            {
                waypoint_list.Add(child.gameObject);
            }
        }
    }

    Vector3 PickRandomWaypoint(Vector3 lastDestination)
    {
        Vector3 new_destination = lastDestination;
        while(new_destination.Equals(lastDestination))
        {
            new_destination = waypoint_list[UnityEngine.Random.Range(0, waypoint_list.Count - 1)].transform.position;
        }
        return new_destination;
    }

    private void SetNewRandomDestination()
    {
        curr_destination = PickRandomWaypoint(curr_destination);
        agent.SetDestination(curr_destination);
        IsWalking = true;
    }

    private float GetRandomWaitTime()
    {
        if (maxWaitTime >= minWaitTime)
            return UnityEngine.Random.Range(minWaitTime, maxWaitTime);
        else
            return 0f;
    }

}
