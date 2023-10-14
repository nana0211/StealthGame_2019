using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MovingWallController : MonoBehaviour
{
    // Start is called before the first frame update
    public float movingSpeed = 15f;
    public float acceleration = 0.5f;
    public Transform[] wayPoints;
    
    [HideInInspector]
    public int currentWayPointIndex;
    
    [HideInInspector]
    public Vector3 currentDestination;
    
    [HideInInspector]
    public bool playerIsOnTrigger = false;
    
    void Awake()
    {
        if (transform.gameObject.CompareTag("MovingWall"))
        {
            playerIsOnTrigger = true;
        }
        currentDestination = wayPoints[0].position;
        //currentWayPointIndex = 0;
    }
    // Update is called once per frame
    void Update()
    {
       if(playerIsOnTrigger)
        WallMoveOnPath();
    }
    void WallMoveOnPath()
    {
        if(isArrivedDestination(currentDestination))
        {
            // Change Destination 
            currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Length;
            currentDestination = wayPoints[currentWayPointIndex].position;
        }
        else
        {
            // Continue Moving 
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, (movingSpeed + acceleration * Time.deltaTime )*Time.deltaTime);

        }
    }
    private bool isArrivedDestination(Vector3 destination)
    {
        return Vector3.Distance(transform.position,destination) < 0.01f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            playerIsOnTrigger = true;
        }
    }
}
