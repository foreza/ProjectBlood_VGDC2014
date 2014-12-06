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
		if(other.tag == "Player" && other.gameObject.GetComponent<PlayerController>().state==PlayerState.NORMAL)
		{
			enemy.OnPlayerSighted();
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
