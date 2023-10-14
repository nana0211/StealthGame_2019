using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


[ExecuteInEditMode]
[Serializable]
[JsonObject(IsReference = true)]
public class VisionCone : MonoBehaviour
{
    public float viewRadius;

    [Range(0,360)]
    public float viewAngle;
    public float meshResolution;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public MeshFilter viewMeshFilter;
    private Mesh viewMesh;
    
    [HideInInspector] public Transform playerTarget = null;

    public Material NeutralStateMaterial;
    public Material AlertStateMaterial;

    public Material AlertStateVisualMaterial;
    public Material NeutralStateVisualMaterial;

    [JsonIgnore]
    public StateController controller;

    void Start()
    {
        controller = GetComponentInParent<StateController>();
        viewMesh = new Mesh
        {
            name = "View Mesh"
        };
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine("FindTargetsWithDelay", .0f);
    }

    void LateUpdate()
    {
        DrawFieldOfView();
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if(!controller.isSleeping)

                FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        if(targetsInViewRadius.Length > 0)
        {
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform; // Get the Target!
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                // See if Player is in the view Angle 
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    // See if there is an Obstacle in the way
                    float distToTarget = Vector3.Distance(transform.position, target.position); // Get distance from pos to target
                    if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                    {
                        if (playerTarget == null)
                        {
                            gameObject.GetComponent<Renderer>().material = AlertStateMaterial;
                            viewMeshFilter.gameObject.GetComponent<Renderer>().material = AlertStateVisualMaterial;
                            playerTarget = target;
                        }
                    }
                    else if (playerTarget != null)
                    {
                        gameObject.GetComponent<Renderer>().material = NeutralStateMaterial;
                        viewMeshFilter.gameObject.GetComponent<Renderer>().material = NeutralStateVisualMaterial;
                        playerTarget = null;
                    }
                }
                else if (playerTarget != null)
                {
                    gameObject.GetComponent<Renderer>().material = NeutralStateMaterial;
                    viewMeshFilter.gameObject.GetComponent<Renderer>().material = NeutralStateVisualMaterial;
                    playerTarget = null;
                }
            }
        }
        else if (playerTarget != null)
        {
            gameObject.GetComponent<Renderer>().material = NeutralStateMaterial;
            viewMeshFilter.gameObject.GetComponent<Renderer>().material = NeutralStateVisualMaterial;
            playerTarget = null;
        }
    }

    void DrawFieldOfView()
    {
        // Number of Rays Available depending on the view angle size and resolution
        int rayCount = Mathf.RoundToInt(viewAngle * meshResolution);
        // Angle Spacing between the different rays
        float spaceAngleSize = viewAngle / rayCount;

        List<Vector3> viewPoints = new List<Vector3>();

        for(int i = 0; i <= rayCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + spaceAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.endPoint);
        }
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount-2) * 3];

        vertices[0] = Vector3.zero;
        for(int i = 0; i < vertexCount-1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if(i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if(Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 endPoint;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _endPoint, float _dst, float _angle)
        {
            hit = _hit;
            endPoint = _endPoint;
            dst = _dst;
            angle = _angle;
        }
    }

    public MeshRenderer GetMeshRenderer()
    {
        return viewMeshFilter.gameObject.GetComponent<MeshRenderer>();
    }

    public GameObject GetMeshObject()
    {
        return viewMeshFilter.gameObject;
    }
}
