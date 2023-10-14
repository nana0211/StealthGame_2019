//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class ObjectiveEventHandler : MonoBehaviour
//{

//    public void NextWaypoint(StateController controller, PatrolAction patrol)
//    {
//        StartCoroutine(patrol.WaitthenNextWaypoint(controller));
//    }

//    void Update(StateController controller, PatrolAction patrol)
//    {
//        if (controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending)
//        {
//            NextWaypoint(controller, patrol);
//            //StartCoroutine(patrol.WaitthenNextWaypoint(controller));
//        }
//    }
//}