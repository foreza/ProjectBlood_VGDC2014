using UnityEngine;
using System.Collections;



public class NoisemakerPlayer : MonoBehaviour {
	private string[] layers = {"Mobs"};
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Noise(float radius)
	{
		RaycastHit2D[] hits  = Physics2D.CircleCastAll(transform.position, radius, new Vector2(0,0), 0, LayerMask.GetMask(layers));
		foreach (RaycastHit2D hit in hits)
		{
			if (hit.collider.tag == "Enemy") {
				Debug.Log ("Did you hear that?", hit.collider);
			}
		}
	}
}

