using UnityEngine;
using System.Collections;

public class LineOfSight : MonoBehaviour
{
	private Enemy enemy;
	
	void Start()
	{
		enemy = this.transform.parent.gameObject.GetComponent<Enemy>();
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player" && other.gameObject.GetComponent<Player>().state==PlayerState.NORMAL)
		{
			enemy.OnPlayerSighted();
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(other.tag == "Player" && other.gameObject.GetComponent<Player>().state==PlayerState.STEALTH)
			{
				enemy.OnPlayerLost();
			}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			enemy.OnPlayerLost();
		}
	}
}
