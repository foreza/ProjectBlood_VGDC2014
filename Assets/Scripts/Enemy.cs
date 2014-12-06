using UnityEngine;
using System.Collections;

public enum EnemyState
{
	PATROL,
	CHASING
}

public class Enemy : Character 
{
	public Player player;
	public GameObject[] patrolPath;
	public EnemyState state;
	
	void Awake()
	{
		this.health = 100;
		this.speed = 50;
	}
	void Start () 
	{
		player = GameObject.Find("Player").GetComponent<Player> ();
		StartCoroutine("Patrol");
	}
	
	// Update is called once per frame
	void Update () 
	{
		//followPlayer();
		
	}
	
	IEnumerator FollowPlayer()
	{
		this.state = EnemyState.CHASING;
		while(this.state == EnemyState.CHASING)
		{
			Vector3 direction = player.transform.position - this.transform.position;
			this.transform.Translate(Vector3.ClampMagnitude(direction, speed*Time.deltaTime), Space.World);
			this.transform.right = player.transform.position - this.transform.position;
			yield return new WaitForFixedUpdate();
		}
	}
	
	IEnumerator Patrol()
	{
		state = EnemyState.PATROL;
		
		int i = 0;
		while(state == EnemyState.PATROL)
		{
			
			Vector2 to = patrolPath[i].transform.position;
			
			while( (Vector2)this.transform.position != to && state == EnemyState.PATROL )
			{
				Vector2 from = this.transform.position;
				float fracJourney = speed * Time.deltaTime / (to - from).magnitude;
				//distTravel = distTravel + fracJourney;
				this.transform.position = Vector2.Lerp(from,to,fracJourney);
				this.transform.right = to - from;
				yield return null;
			}
			
			
			i = (i >= patrolPath.Length - 1)? 0 : ++i;
		}
	}
	
	public void OnPlayerSighted()
	{
		if(state != EnemyState.CHASING)
		{
			StartCoroutine("FollowPlayer");
		}
		
	}
	
	public void OnPlayerLost()
	{
		if(state == EnemyState.CHASING)
		{
			StartCoroutine("Patrol");
		}
	}
}

