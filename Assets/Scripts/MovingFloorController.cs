using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloorController : MonoBehaviour
{
    // Start is called before the first frame update
    public float movingSpeed = 8f;
    public float acceleration = 0.5f;
    public Transform[] wayPoints;

    [HideInInspector]
    public int currentWayPointIndex;
    
    [HideInInspector]
    public Vector3 currentDestination;
    
    [HideInInspector]
    public bool playerIsOnTrigger = false;
    
    public FirstPersonControllerTank player;
    
    private Vector3 offSet =  new Vector3(1f, 1f,1f);
    void Awake()
    {
        //currentDestination = Vector3.Scale(wayPoints[0].position, offSet);
        currentDestination = wayPoints[0].position;

        //Debug.Log("current dest " + currentDestination);
        //Debug.Log("player is invisible " + player.IsInvisible);
        //Debug.Log("player is locked " + player.isLocked);
 
        //currentWayPointIndex = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (playerIsOnTrigger)
        {
            WallMoveOnPath();

        }

    }
    void WallMoveOnPath()
    {
        if (isArrivedDestination(currentDestination))
        {
            // Unbound the player
            playerIsOnTrigger = false;
            player.isLocked = false;
            // Destroy the movable floor after 1s. 
            Destroy(gameObject, 1f);
           
        }
        else
        {
            // Continue Moving 
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, (movingSpeed + acceleration * Time.deltaTime) * Time.deltaTime);
            Debug.Log("current dest 2 " + currentDestination);

        }
        player.transform.position = transform.position ;
    }
    private bool isArrivedDestination(Vector3 destination)
    {
        return Vector3.Distance(transform.position, destination) < 0.01f;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("player is invisible " + player.IsInvisible);
        Debug.Log("player is locked " + player.isLocked);
        if (other.CompareTag("Player"))
        {
            if (!player.isLocked)
            {
                playerIsOnTrigger = true;
                //player cannot manually move themselves until it reach to the destination. 
                player.isLocked = true;
            }
        }
    }
}
