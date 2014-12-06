using UnityEngine;
using System.Collections;

public enum PlayerState
{
	NORMAL,
	STEALTH
}


public class Player : Character 
{
	float energy;
	public PlayerState state;
	public SpriteRenderer sprite;

	void Start () 
	{
		sprite = transform.FindChild ("PlayerPlaceholder").GetComponent<SpriteRenderer>();
		state = PlayerState.NORMAL;
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
		Application.LoadLevel(0);
		// StartCoroutine("LoadStartScreen"); 

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

	IEnumerator LoadStartScreen() { // not working as intended oh well.
	
		yield return new WaitForSeconds(1);


		Application.LoadLevel(0);
	}


}
