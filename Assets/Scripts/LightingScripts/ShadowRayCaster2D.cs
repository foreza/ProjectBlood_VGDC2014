using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowRayCaster2D : MonoBehaviour
{
    public GameObject lightmeshholder;

    public float distance = 1000;
    public bool debugLines = true;

    Camera parentCamera;
    List<PolygonCollider2D> polygonColliders = new List<PolygonCollider2D>();
    List<BoxCollider2D> boxColliders = new List<BoxCollider2D>();
    List<CircleCollider2D> circleColliders = new List<CircleCollider2D>();

    private Vector3[] newVertices;
    private Vector3[] vertexList;
    private int[] triangles;
    private Mesh mesh;

    private LayerMask mask; 
    private string[] layers = {"Enemy","LightWalls"};

    // Use this for initialization
    void Start()
    {
        parentCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mask = LayerMask.GetMask(layers);

        //fill the collider lists with the appropriate colliders
        polygonColliders = new List<PolygonCollider2D>(FindObjectsOfType<PolygonCollider2D>());
        boxColliders = new List<BoxCollider2D>(FindObjectsOfType<BoxCollider2D>());
        circleColliders = new List<CircleCollider2D>(FindObjectsOfType<CircleCollider2D>());

//        foreach (PolygonCollider2D collider in GameObject.Find("LineOfSight").GetComponents<PolygonCollider2D>())
//        {
//            polygonColliders.Remove(collider);
//        }

//        newVertices = new Vector3[GetVertices().Length];
        mesh = lightmeshholder.GetComponent<MeshFilter>().mesh;
        BuildMesh();
    }
    
    // Update is called once per frame, after all other tasks within that frame have been run
    void LateUpdate()
    {
        Vector2[] worldVertexList = GetVertices();
        foreach (Vector2 point in worldVertexList)
        {
            Vector2 direction = ((Vector2)point - (Vector2)(transform.position));
            Vector2 tempPoint;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, mask);
            if (hit.collider != null)
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);
                tempPoint = lightmeshholder.transform.InverseTransformPoint(hit.point);
            } else
            {
                tempPoint = distance * direction;
                Debug.DrawLine(transform.position, tempPoint, Color.yellow);
            }
        }
    }

    void BuildMesh()
    {
        Vector2[] worldVertexList = GetVertices();

        foreach (Vector2 point in worldVertexList)
        {
            Vector2 direction = (Vector2)point - (Vector2)transform.position; 
            Vector2 tempPoint;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
            if (hit.collider != null)
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);
                //tempPoint = lightmeshholder.transform.InverseTransformPoint(hit.point);
            } else
            {
                Debug.DrawLine(transform.position, (Vector2)transform.position + (distance * direction), Color.yellow);
            }
        }
    }

    // Returns an unsorted list of the vertices of every 2D collider within the current scene.
    Vector2[] GetVertices()
    {
        List<Vector2> points = new List<Vector2>();

        foreach (PolygonCollider2D polyCollider in polygonColliders)
        {
            for (int i = 0; i < polyCollider.pathCount; i++)
            {
                foreach (Vector2 point in polyCollider.GetPath(i))
                {
                    //points.Add(point);
                    if (Vector2.Distance(transform.position, point) < distance)
                    {
                        points.Add(point);
                    }
                }
            }
        }


//        foreach (BoxCollider2D boxCollider in boxColliders)
//        {
//            Vector2 size = boxCollider.size;
//            Vector2 center = boxCollider.center;
//
//            points.Add(new Vector2(center.x + (size.x / 2), center.y + (size.y / 2)));
//            points.Add(new Vector2(center.x - (size.x / 2), center.y + (size.y / 2)));
//            points.Add(new Vector2(center.x + (size.x / 2), center.y - (size.y / 2)));
//            points.Add(new Vector2(center.x - (size.x / 2), center.y - (size.y / 2)));
//        }

        foreach (CircleCollider2D circleCollider in circleColliders)
        {
            points.Add(circleCollider.center);
        }


        foreach (Vector2 point in points)
        {
            //Debug.DrawLine(transform.position, point, Color.white);
        }


        return points.ToArray();
    }
}
