using UnityEngine;
using System.Collections;



public class NoisemakerPlayer : MonoBehaviour {
	private string[] layers = {"LightWalls","FloorDecor"};
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Noise (10000);
	}

	public void Noise(float radius)
	{
		RaycastHit2D[] hits  = Physics2D.CircleCastAll(transform.position, radius, new Vector2(0,0), 0);
		foreach (RaycastHit2D hit in hits)
		{

			if (hit.transform.tag == "Enemy") {
				Debug.Log ("Did you hear that?", hit.collider);
			}
		}
	}
}

