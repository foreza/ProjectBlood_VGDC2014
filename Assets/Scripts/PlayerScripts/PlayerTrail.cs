using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTrail : MonoBehaviour 
{
	public float spawnRate = 1.0f;
	public float lifeTime = 5.0f;
	
	public LinkedList<TrailCrumb> trail;
	
	private GameObject trailObj;
	private float lastSpawnTime;

	
	void Start()
	{
		trail = new LinkedList<TrailCrumb>();
		trailObj = Resources.Load("TrailObject") as GameObject;
		lastSpawnTime = 0.0f;
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.time > lastSpawnTime + spawnRate)
		{
			lastSpawnTime = Time.time;
			GameObject crumbObj = Instantiate(trailObj) as GameObject;
			crumbObj.transform.position = this.transform.position;
			trail.AddFirst(crumbObj.GetComponent<TrailCrumb>());
		}
		
		if(trail.Count > 0 && trail.Last.Value.GetLifeTime() > lifeTime)
		{
			GameObject toDestroy = trail.Last.Value.gameObject;
			trail.RemoveLast();
			Destroy(toDestroy);
		}
	}
}
