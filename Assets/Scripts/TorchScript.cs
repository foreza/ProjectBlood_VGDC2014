using UnityEngine;
using System.Collections;

public class TorchScript : MonoBehaviour
{

        public GameObject lightmeshholder;
        public bool lit=true;
        public int RaysToShoot; //64; 128; 1024; 
        float distance;
        private Vector3[] vertices;
        private Vector2[] vertices2d;
        private int[] triangles;
        private Vector2[] uvs;
        private Vector3[] newvertices;
        private Mesh mesh;
        private string[] layers = { "LightWalls" };
		// Use this for initialization

		void Start ()
		{
            distance = GetComponent<CircleCollider2D>().radius;
            vertices2d = new Vector2[RaysToShoot];
            //triangles = new int[RaysToShoot];
            //	vertices2 = new Vector3[4];
            mesh = lightmeshholder.GetComponent<MeshFilter>().mesh;
            BuildMesh();
            lit = !lit;
            ToggleLight();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<Enemy>().BoostSight();
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<Enemy>().NormalSight();
            }
        }


		public bool IsLit()
		{
		return lit;
		}
        public void ToggleLight()
        {
            if (!lit)
            {
                Ignite();
            }
            else
            {
                Extinguish();
            }
        }
        void Ignite()
        {
            if (!lit)
            {
                lit = true;
                mesh.vertices = newvertices;
                mesh.triangles = triangles;
                mesh.uv = uvs;
            }
        }
        void Extinguish()
        {
            if (lit)
            {
                lit = false;
                mesh.vertices = new Vector3[0];
                mesh.triangles = new int[0];
                mesh.uv = new Vector2[0];
            }
        }
        void BuildMesh()
        {

            // dont cast if not moved?
            // build prelook-array of hit points/pixels/areas?
            // skip duplicate hit points (compare previous)
            // always same amount of vertices, no need create new mesh?..but need to triangulate or not??

            float angle = 0;
            for (int i = 0; i < RaysToShoot; i++)
            {
                float x = Mathf.Sin(angle);
                float y = Mathf.Cos(angle);
                angle += 2 * Mathf.PI / RaysToShoot;

                Vector3 dir = new Vector3(x, y, 0);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distance, LayerMask.GetMask(layers));
                Vector3 tmp;
                if (hit.collider != null)
                {
                    tmp = (lightmeshholder.transform).InverseTransformPoint(hit.point);
                    vertices2d[i] = new Vector3(tmp.x, tmp.y, 0);
                }
                else
                {
                    tmp = lightmeshholder.transform.InverseTransformPoint(transform.position + (distance * dir));
                    vertices2d[i] = new Vector3(tmp.x, tmp.y, 0);
                }
            }

            // triangulate.cs
            //    var tr : Triangulator = new Triangulator(vertices2d);
            //    var indices : int[] = tr.Triangulate();

            // build mesh
            uvs = new Vector2[vertices2d.Length + 1];
            newvertices = new Vector3[vertices2d.Length + 1];
            for (int n = 0; n < newvertices.Length - 1; n++)
            {
                newvertices[n] = new Vector3(vertices2d[n].x, vertices2d[n].y, 0);

                // create some uv's for the mesh?
                // uvs[n] = vertices2d[n];

            }

            //print("len"+newvertices.Length+" n:"+n);

            triangles = new int[newvertices.Length * 3];

            // triangle list
            int iterator = -1;
            for (int n = 0; n < triangles.Length - 3; n += 3)
            {
                iterator++;
                triangles[n] = newvertices.Length - 1;
                if (iterator >= newvertices.Length)
                {
                    triangles[n + 1] = 0;
                    //print ("hit:"+i);
                }
                else
                {
                    triangles[n + 1] = iterator + 1;
                }
                triangles[n + 2] = iterator;
            }
            iterator++;
            // central point
            newvertices[newvertices.Length - 1] = new Vector3(0, 0, 0);
            triangles[triangles.Length - 3] = newvertices.Length - 1;
            triangles[triangles.Length - 2] = 0;
            triangles[triangles.Length - 1] = iterator - 1;

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

