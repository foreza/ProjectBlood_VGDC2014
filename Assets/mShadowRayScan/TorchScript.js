// raycast light test 2.0 - mgear - http://unitycoder.com/blog
// update from the previous version,
// whats new: mesh is build at Start()
// then in the Update() we only move the vertices, no need to triangulate or create uv maps in every frame


#pragma strict

public var lightmeshholder:GameObject;

public var RaysToShoot:int=256; //64; 128; 1024; 
public var distance:int=150;
public var lit=true;
private var vertices : Vector3[];
private var vertices2d : Vector2[];
private var triangles : int[];
private var uvs : Vector2[];
private var newvertices : Vector3[];
//private var vertices2 : Vector3[];
private var mesh : Mesh;
private var layers : String[] = ["LightWalls"];

function Start () 
{


	//vertices = new Vector3[RaysToShoot];
	vertices2d = new Vector2[RaysToShoot];
	//triangles = new int[RaysToShoot];
//	vertices2 = new Vector3[4];
	mesh= lightmeshholder.GetComponent(MeshFilter).mesh;
	BuildMesh ();
	lit = !lit;
	ToggleLight();

}

function Update () 
{


}

function ToggleLight()
{
    if(!lit)
    {
    	Ignite();
    }
    else
    {
    	Extinguish();
    }
}

function Ignite()
{
    if(!lit)
    {
    	lit = true;
		mesh.vertices = newvertices;
   		mesh.triangles = triangles;
   		mesh.uv = uvs;
   	}
}
function Extinguish()
{
    if(lit)
    {
    	lit = false;
		mesh.vertices = new Vector3[0];
   		mesh.triangles = new int[0];
   		mesh.uv = new Vector2[0];
   	}
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
		
		var dir:Vector2 = Vector2(x,y);
		var hit : RaycastHit2D = Physics2D.Raycast (transform.position, dir, distance, LayerMask.GetMask(layers));
		if(hit.collider != null)
		{
			var tmp = lightmeshholder.transform.InverseTransformPoint(hit.point);
			vertices2d[i] = Vector3(tmp.x,tmp.y,0);
		}
		else
		{
			tmp = lightmeshholder.transform.InverseTransformPoint(transform.position+(distance*dir));
			vertices2d[i] = Vector3(tmp.x,tmp.y,0);
		}
	}

	// triangulate.cs
//    var tr : Triangulator = new Triangulator(vertices2d);
//    var indices : int[] = tr.Triangulate();
	
	// build mesh
    uvs = new Vector2[vertices2d.Length+1];
    newvertices = new Vector3[vertices2d.Length+1];
    for (var n : int = 0; n<newvertices.Length-1;n++) 
	{
        newvertices[n] = new Vector3(vertices2d[n].x, vertices2d[n].y, 0);

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
