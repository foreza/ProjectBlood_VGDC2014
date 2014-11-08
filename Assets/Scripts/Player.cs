using UnityEngine;
using System.Collections;

public class Player : Character 
{
	float energy;

	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(this.health <= 0)
		{
			killPlayer();
		}
	}

	void killPlayer()
	{
		this.gameObject.SetActive(false);
	}

	void takeHit(float dmg)
	{
		this.health = this.health - dmg;
		if(this.health < 0)
		{
			this.health = 0;
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.tag == "Enemy")
		{
			takeHit(10);
			Debug.Log("Health: " + this.health);
		}
	}
}
