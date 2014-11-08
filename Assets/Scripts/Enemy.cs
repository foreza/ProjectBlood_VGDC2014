using UnityEngine;
using System.Collections;

public class Enemy : Character 
{
	public Player player;

	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		followPlayer();
	}

	void followPlayer()
	{
		Vector3 direction = player.gameObject.transform.position - this.gameObject.transform.position;
		Debug.Log("x: " + direction.x +  "  y: " + direction.y);
		this.gameObject.transform.Translate(Vector3.ClampMagnitude(direction, speed*Time.deltaTime));
	}
}
