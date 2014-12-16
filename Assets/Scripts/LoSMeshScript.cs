using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LoSMeshScript : MonoBehaviour
{

	public GameObject lightmeshholder;
	
	public int RaysToShoot; //64; 128; 1024; 
	public int distance;
	private Vector3[] vertices;
	private Vector2[] vertices2d;
	private int[] triangles;
	//private var vertices2 : Vector3[];
	private Mesh mesh;
	private LayerMask mask; 
	private string[] layers = {"Enemy","LightWalls"};
	private List<Transform> visibleMobs; //keeps track of the mobs in the LoS
	
	void Start () 
	{
		mask = LayerMask.GetMask (layers);
		Debug.Log (mask.value);
		vertices2d = new Vector2[RaysToShoot];
		mesh= lightmeshholder.GetComponent<MeshFilter>().mesh;
		visibleMobs = new List<Transform> ();
		BuildMesh ();
		
	}
	
	void LateUpdate () 
	{
		List<Transform> sightedMobs = new List<Transform> ();
		vertices = mesh.vertices;
		GameObject[] newMobs = {};
		float angle = 0;
		for (int i=0;i<RaysToShoot;i++)
		{
			float x = Mathf.Sin(angle);
			float y = Mathf.Cos(angle);
			angle += 2*Mathf.PI/RaysToShoot;
			
			Vector3 dir = new Vector3(x,y,0);
			RaycastHit2D hit  = Physics2D.Raycast (transform.position, dir, distance, mask);
			Vector3 tmp;
			if(hit.collider != null)
			{
				tmp = lightmeshholder.transform.InverseTransformPoint(hit.point);
				vertices[i] = new Vector3(tmp.x,tmp.y,0);

				//code for LoS showing enemies
				if (hit.collider.tag == "Enemy")
				{
					if(!visibleMobs.Contains (hit.transform))//&& visibleMobs.Contains(hit.transform.parent.gameObject)) 
					{
						hit.transform.Find ("EnemyPlaceholder").GetComponent<SpriteRenderer>().enabled = true;
						visibleMobs.Add (hit.transform);
					}
					else if(!sightedMobs.Contains (hit.transform))
					{
						sightedMobs.Add (hit.transform);
                        //Debug.Log(hit.GetHashCode());
					}
				}
			}
			else
			{
				tmp = (lightmeshholder.transform).InverseTransformPoint(transform.position+(distance*dir));
				vertices[i] = new Vector3(tmp.x,tmp.y,0);
			}
			//		}else{ // no hit
			//			Debug.DrawRay (transform.position, dir*distance, Color(1,1,0,1));
			//			var tmp2 = lightmeshholder.transform.InverseTransformPoint(transform.position+(i*dir));
			//			vertices[i] = Vector3(tmp2.x,tmp2.y,0);
		}
		
		// last vertice is at the player location (center point)
		vertices[vertices.Length - 1] = lightmeshholder.transform.InverseTransformPoint(transform.position);
		
		mesh.vertices = vertices;

		//code for LoS showing enemies
		foreach (Transform trans in visibleMobs)
		{
			if(!sightedMobs.Contains (trans))
			{
				//visibleMobs.Remove (trans);
				trans.Find ("EnemyPlaceholder").GetComponent<SpriteRenderer>().enabled = false;
			}
		}
        visibleMobs = sightedMobs;
		
	}
	
	void BuildMesh () 
	{
		
		// dont cast if not moved?
		// build prelook-array of hit points/pixels/areas?
		// skip duplicate hit points (compare previous)
		// always same amount of vertices, no need create new mesh?..but need to triangulate or not??
		
		float angle = 0;
		for (int i=0;i<RaysToShoot;i++)
		{
			float x = Mathf.Sin(angle);
			float y = Mathf.Cos(angle);
			angle += 2*Mathf.PI/RaysToShoot;
			
			Vector3 dir = new Vector3(x,y,0);
			RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, distance);
			Vector3 tmp;
			if(hit.collider != null)
			{
				tmp = (lightmeshholder.transform).InverseTransformPoint(hit.point);
				vertices2d[i] = new Vector3(tmp.x,tmp.y,0);
			}
			else
			{
				tmp = lightmeshholder.transform.InverseTransformPoint(transform.position+(distance*dir));
				vertices2d[i] = new Vector3(tmp.x,tmp.y,0);
			}
		}
		
		// triangulate.cs
		//    var tr : Triangulator = new Triangulator(vertices2d);
		//    var indices : int[] = tr.Triangulate();
		
		// build mesh
		Vector2[] uvs  = new Vector2[vertices2d.Length+1];
		Vector3[] newvertices = new Vector3[vertices2d.Length+1];
		for (int n = 0; n<newvertices.Length-1;n++) 
		{
			newvertices[n] = new Vector3(vertices2d[n].x, vertices2d[n].y, 0);
			
			// create some uv's for the mesh?
			// uvs[n] = vertices2d[n];
			
		}
		
		//print("len"+newvertices.Length+" n:"+n);
		
		triangles = new int[newvertices.Length*3];
		
		// triangle list
		int iterator = -1;
		for (int n=0;n<triangles.Length-3;n+=3)
		{
			iterator++;
			triangles[n] = newvertices.Length-1;
			if (iterator>=newvertices.Length)
			{
				triangles[n+1] = 0;
				//print ("hit:"+i);
			}else{
				triangles[n+1] = iterator+1;
			}
			triangles[n+2] = iterator;
		}    
		iterator++;
		// central point
		newvertices[newvertices.Length-1] = new Vector3(0,0,0);
		triangles[triangles.Length-3] = newvertices.Length-1;
		triangles[triangles.Length-2] = 0;
		triangles[triangles.Length-1] = iterator-1;
		
		// Create the mesh
		//var msh : Mesh = new Mesh();
		mesh.vertices = newvertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		
		//    mesh.RecalculateNormals(); // need?
		//    mesh.RecalculateBounds(); // need ?
		
		// last triangles
		//	triangles[i+1] = 0;
		//	triangles[i+2] = 0;
		//	triangles[i+1] = 0;
		
		//triangles.Reverse();
		
		//	mesh.vertices = newvertices;
		//	mesh.triangles = triangles;
		
		// not every frame? clear texture before take new shot?
		//	if (grab>10) GrabToTexture();
		//	grab++;
		
	}


}