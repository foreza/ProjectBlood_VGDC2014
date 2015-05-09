using UnityEngine;
using System.Collections;

public class LineOfSight : MonoBehaviour
{
	public LayerMask sightMask;

	private Enemy parentEnemy;
	
	void Start()
	{

		parentEnemy = this.transform.parent.gameObject.GetComponent<Enemy>();
		if(parentEnemy == null)
            parentEnemy = this.transform.parent.gameObject.GetComponent<EnemyBoss>();
		string[] layers = {"LightWalls", "Mobs"};
		sightMask = LayerMask.GetMask(layers);
	}

	// If player WALKS into sight.
	void OnTriggerEnter2D(Collider2D other)
	{
		// print ("Enter collider: " + other.name);

		if ( other.gameObject.tag == "Player")
		{
			Player player = other.transform.GetComponent <Player> ();
			//Vector2 rayDir = player.transform.position - this.transform.position;
			//Debug.Log ("Raydir" + rayDir);
			//RaycastHit2D hit = Physics2D.Raycast(this.transform.position, rayDir, 1000, sightMask);

			//Debug.Log (hit.transform.position + "HI THIS IS TO STRING" + player.transform.position);
			//if(hit && hit.transform == player.transform)
			//if

				//Debug.Log ("Player has entered collidor. I SEE YOU. YOU SEE ME.");
				if(player.state != PlayerState.STEALTH)
				{
					this.GetComponent<AudioSource>().Play();
					parentEnemy.OnPlayerSighted();
				}
			else{
				print ("-shrug-");
			}

		}
	}
	// If player leaves sight/stealths.

	// If the player appears out of stealth in front of the enemy.

	void OnTriggerStay2D(Collider2D other)
	{

		Player player = other.transform.GetComponent <Player> ();
		if ( other.gameObject.tag == "Player" && player.state == PlayerState.NORMAL)
		{

			//Debug.Log ("You surprised me!~");
			//this.GetComponent<AudioSource>().Play();
			//parentEnemy.OnPlayerSighted();
		
			
		}
	

	}


}
