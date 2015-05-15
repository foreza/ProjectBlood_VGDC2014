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
		if ( other.gameObject.tag == "Player")
		{
			Player player = other.transform.GetComponent <Player> ();
            
				if(player.state != PlayerState.STEALTH)
				{
					this.GetComponent<AudioSource>().Play();
					parentEnemy.OnPlayerSighted();
				}
		}
	}
}
