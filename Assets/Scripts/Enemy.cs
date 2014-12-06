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
	public SpriteRenderer sprite;
	public SpriteRenderer minimapSprite;
	void Awake()
	{
		this.health = 100;
	}
	void Start () 
	{
		sprite = transform.FindChild ("EnemyPlaceholder").GetComponent<SpriteRenderer>();
		minimapSprite = transform.FindChild ("Minimap EnemyPlaceholder").GetComponent<SpriteRenderer>();
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
		
		int i = closestWaypoint ();
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

	int closestWaypoint()
	{
		int nearest = 0;
		for ( int i = 0; i<this.patrolPath.Length; i++)
		{
			float distance = (this.transform.position - this.patrolPath[i].transform.position).magnitude;
			Debug.Log (distance);
			if(distance <(this.transform.position - this.patrolPath[nearest].transform.position).magnitude)
			{
				nearest = i;
			}
		}
		Debug.Log (nearest);
		return nearest;
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

	public void Die()
	{
		this.audio.Play ();
		this.sprite.enabled = false;
		this.minimapSprite.enabled = false;
		this.collider2D.enabled = false;

	}
}

