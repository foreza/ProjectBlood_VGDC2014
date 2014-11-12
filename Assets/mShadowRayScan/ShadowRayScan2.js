// raycast light test 2.0 - mgear - http://unitycoder.com/blog
// update from the previous version,
// whats new: mesh is build at Start()
// then in the Update() we only move the vertices, no need to triangulate or create uv maps in every frame


#pragma strict

public var lightmeshholder:GameObject;

private var RaysToShoot:int=1024; //64; 128; 1024; 
private var distance:int=1500;
private var vertices : Vector3[];
private var vertices2d : Vector2[];
private var triangles : int[];
//private var vertices2 : Vector3[];
private var mesh : Mesh;



function Start () 
{


	//vertices = new Vector3[RaysToShoot];
	vertices2d = new Vector2[RaysToShoot];
	//triangles = new int[RaysToShoot];
//	vertices2 = new Vector3[4];
	mesh= lightmeshholder.GetComponent(MeshFilter).mesh;

	BuildMesh ();

}

function Update () 
{

	vertices = mesh.vertices;

	var angle:float = 0;
	for (var i:int=0;i<RaysToShoot;i++)
	{
		var x = Mathf.Sin(angle);
		var y = Mathf.Cos(angle);
		angle += 2*Mathf.PI/RaysToShoot;
		
		var dir:Vector3 = Vector3(x,y,0);
		var hit : RaycastHit2D = Physics2D.Raycast (transform.position, dir);
		if(hit.distance == null)
		{
		    if(hit.distance>160)
		    {
		    	var tmp = lightmeshholder.transform.InverseTransformPoint(transform.position+(160*dir));
		    }
		    else
		    {
//			Debug.DrawLine (transform.position, hit.point, Color(1,1,0,1));
				tmp = lightmeshholder.transform.InverseTransformPoint(transform.position+(hit.distance*dir));
			}
			vertices[i] = Vector3(tmp.x,tmp.y,0);
		}else{ // no hit
//			Debug.DrawRay (transform.position, dir*distance, Color(1,1,0,1));
			var tmp2 = lightmeshholder.transform.InverseTransformPoint(transform.position+(80.5f*dir));
			vertices[i] = Vector3(tmp2.x,tmp2.y,0);
		}
	}
	
	// last vertice is at the player location (center point)
	vertices[i] = lightmeshholder.transform.InverseTransformPoint(transform.position);
	
	 mesh.vertices = vertices;
	
	
}

function BuildMesh () 
{

	// dont cast if not moved?
	// build prelook-array of hit points/pixels/areas?
	// skip duplicate hit points (compare previous)
	// always same amount of vertices, no need create new mesh?..but need to triangulate or not??

	var angle:float = 0;
	for (var i:int=0;i<RaysToShoot;i++)
	{
		var x = Mathf.Sin(angle);
		var y = Mathf.Cos(angle);
		angle += 2*Mathf.PI/RaysToShoot;
		
		var dir:Vector3 = Vector3(x,0,y);
		var hit : RaycastHit;
		if (Physics.Raycast (transform.position, dir, hit, distance)) 
		{
//			Debug.DrawLine (transform.position, hit.point, Color(1,1,0,1));
			
			/*
			// bounce ray, ignore last hit object?
			// reflect more than 1 ray?
			var dir2:Vector3 = Vector3.Reflect(dir, hit.normal);
			var hit2 : RaycastHit;
			if (Physics.Raycast (hit.point, dir2, hit2, distance/4)) 
			{
				Debug.DrawLine (hit.point, hit2.point, Color(1,0,0,1));
			}else{ // we might not hit anything, because of short bounce distance
				Debug.DrawRay (hit.point, dir2*(distance/4), Color(1,0,0,1));
			}
			*/
			
			var tmp = lightmeshholder.transform.InverseTransformPoint(hit.point);
			vertices2d[i] = Vector2(tmp.x,tmp.z);
			
			
		}else{ // no hit
//			Debug.DrawRay (transform.position, dir*distance, Color(1,1,0,1));

			var tmp2 = lightmeshholder.transform.InverseTransformPoint(lightmeshholder.transform.position+dir);
			vertices2d[i] = Vector2(tmp2.x,tmp2.z);


		}
	}

	// triangulate.cs
//    var tr : Triangulator = new Triangulator(vertices2d);
//    var indices : int[] = tr.Triangulate();
	
	// build mesh
    var uvs : Vector2[] = new Vector2[vertices2d.Length+1];
    var newvertices : Vector3[] = new Vector3[vertices2d.Length+1];
    for (var n : int = 0; n<newvertices.Length-1;n++) 
	{
        newvertices[n] = new Vector3(vertices2d[n].x, 0, vertices2d[n].y);

	// create some uv's for the mesh?
	// uvs[n] = vertices2d[n];
		
    }
    
	//print("len"+newvertices.Length+" n:"+n);
	
	triangles = new int[newvertices.Length*3];
	    
	// triangle list
	i = -1;
	for (n=0;n<triangles.length-3;n+=3)
	{
		i++;
		triangles[n] = newvertices.Length-1;
		if (i>=newvertices.Length)
		{
			triangles[n+1] = 0;
			//print ("hit:"+i);
		}else{
			triangles[n+1] = i+1;
		}
		triangles[n+2] = i;
	}    
    i++;
	// central point
	newvertices[newvertices.Length-1] = new Vector3(0,0,0);
	triangles[triangles.length-3] = newvertices.Length-1;
	triangles[triangles.length-2] = 0;
	triangles[triangles.length-1] = i-1;
   
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
