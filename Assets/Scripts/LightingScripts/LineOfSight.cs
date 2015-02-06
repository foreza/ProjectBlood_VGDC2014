using UnityEngine;
using System.Collections;

public class LineOfSight : MonoBehaviour
{
	public LayerMask sightMask;

	private Enemy parentEnemy;
	
	void Start()
	{

		parentEnemy = this.transform.parent.gameObject.GetComponent<Enemy>();
		
		string[] layers = {"LightWalls", "Mobs"};
		sightMask = LayerMask.GetMask(layers);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(parentEnemy.state == EnemyState.PATROL && other.tag == "Player")
		{
			Player player = other.transform.GetComponent<Player>();
			Vector2 rayDir = player.transform.position - this.transform.position;
			RaycastHit2D hit = Physics2D.Raycast(this.transform.position, rayDir, 1000, sightMask);
			
			if(hit && hit.transform == player.transform)
			{
				if(player.state != PlayerState.STEALTH)
				{
					this.audio.Play();
					parentEnemy.OnPlayerSighted();
				}
			}
		}
	}

}
