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

	void OnTriggerEnter2D(Collider2D other)
	{
		if(parentEnemy.state == EnemyState.PATROL && other.tag == "Player")
		{
			Player player = other.transform.GetComponent<Player>();
			Vector2 rayDir = player.transform.position - this.transform.position;
			//Debug.Log ("Raydir" + rayDir);
			//RaycastHit2D hit = Physics2D.Raycast(this.transform.position, rayDir, 1000, sightMask);

			//Debug.Log (hit.transform.position + "HI THIS IS TO STRING" + player.transform.position);
			//if(hit && hit.transform == player.transform)
			//if

				Debug.Log ("I SEE YOU. YOU SEE ME. WE'RE A HAPPY VAMPIRIC FAMILY");
				if(player.state != PlayerState.STEALTH)
				{
					this.audio.Play();
					parentEnemy.OnPlayerSighted();
					
					Debug.Log ("I SEE YOU. YOU SEE ME. WE'RE A HAPPY VAMPIRIC FAMILY");
				}

		}
	}

}
