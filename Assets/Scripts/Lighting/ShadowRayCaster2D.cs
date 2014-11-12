using UnityEngine;
using System.Collections;
#pragma strict

public class ShadowRayCaster2D : MonoBehaviour {
    public GameObject lightmesholder;

    private int maxRaysToCast;
    private float maxDistance = 15;
    private Vector2[] verticies2d;
    private int[] triangles;
    private Mesh mesh;

	// Use this for initialization
	void Start () 
    {
        verticies2d = new Vector2[maxRaysToCast];
        mesh = lightmesholder.GetComponent<MeshFilter>().mesh;

        BuildMesh();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void BuildMesh()
    {
        float angle = 0;
        Collider2D[] colliders = FindObjectsOfType<PolygonCollider2D>();
        foreach (PolygonCollider2D collider in colliders)
        {
            foreach(Vector2 point in collider.points)
            {
                Debug.Log(point.x.ToString() + " " + point.y.ToString());
            }
        }
    }
}
