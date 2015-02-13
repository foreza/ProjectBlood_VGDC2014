using UnityEngine;
using System.Collections;


public class EnemyBoss : Enemy
{
	// PUBLIC VARIABLES OVERRIDE
	// public GameObject[] patrolPath
	
	// PRIVATE VARIABLES

	// INITIALIZE
	void Start () 
	{

	}
	
	// FIXED UPDATE
	void FixedUpdate ()
	{
		testSkill();
	}

	void testSkill()
	{
				Debug.Log ("I'm using a skill~");
		}

}


