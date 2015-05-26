using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowRayCaster2D : MonoBehaviour
{
    public GameObject lightmeshholder;

    public float distance = 1000;

    List<PolygonCollider2D> polygonColliders = new List<PolygonCollider2D>();
    List<CircleCollider2D> circleColliders = new List<CircleCollider2D>();

    private Vector3[] newVertices;
    private Vector3[] vertexList;
    private int[] triangles;

    // Use this for initialization
    void Start()
    {
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
                    if (Vector2.Distance(transform.position, point) < distance)
                    {
                        points.Add(point);
                    }
                }
            }
        }

        foreach (CircleCollider2D circleCollider in circleColliders)
        {
            points.Add(circleCollider.offset);
        }

        return points.ToArray();
    }
}
