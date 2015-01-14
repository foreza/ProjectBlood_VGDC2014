using UnityEngine;
using System.Collections;

public class TrailCrumb : MonoBehaviour {

	private float lifeTime;
	
	void Start ()
	{
		lifeTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		lifeTime = lifeTime + Time.deltaTime;
	}
	
	public float GetLifeTime()
	{
		return lifeTime;
	}
}
