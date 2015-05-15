using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowRayCaster2D : MonoBehaviour
{
    public GameObject lightmeshholder;

    public float distance = 1000;
    public bool debugLines = true;

    List<PolygonCollider2D> polygonColliders = new List<PolygonCollider2D>();
    List<CircleCollider2D> circleColliders = new List<CircleCollider2D>();

    private Vector3[] newVertices;
    private Vector3[] vertexList;
    private int[] triangles;

    private LayerMask mask; 
    private string[] layers = {"Enemy","LightWalls"};

    // Use this for initialization
    void Start()
    {
        mask = LayerMask.GetMask(layers);

        //fill the collider lists with the appropriate colliders
        polygonColliders = new List<PolygonCollider2D>(FindObjectsOfType<PolygonCollider2D>());
        circleColliders = new List<CircleCollider2D>(FindObjectsOfType<CircleCollider2D>());
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
            points.Add(circleCollider.offset);
        }

        return points.ToArray();
    }
}
