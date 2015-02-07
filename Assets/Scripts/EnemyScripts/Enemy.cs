using UnityEngine;
using System.Collections;

public enum EnemyState
{
	PATROL,
	CHASING,
    DEAD
}

enum EnemyVisionState
{
    NORMAL,
    BOOSTED
}

public class Enemy : Character 
{
	public GameObject[] patrolPath;
	public EnemyState state;
	
	private Player player;
	private PlayerTrail playerTrail;
    private EnemyVisionState vision = EnemyVisionState.NORMAL;
	private SpriteRenderer sprite;
	private SpriteRenderer minimapSprite;
	private  Transform LoSCollider;
	private LayerMask trackMask;
	private LayerMask sightMask;
	
	void Start () 
	{
		sprite = transform.FindChild ("EnemyPlaceholder").GetComponent<SpriteRenderer>();
		minimapSprite = transform.FindChild ("Minimap EnemyPlaceholder").GetComponent<SpriteRenderer>();
        LoSCollider = transform.FindChild("LineOfSight");
		player = GameObject.Find("Player").GetComponent<Player> ();
		playerTrail = player.GetComponent<PlayerTrail>();
		
		string[] trackLayers = {"LightWalls", "Tracks"};
		trackMask = LayerMask.GetMask(trackLayers);
		string[] sightLayers = {"LightWalls", "Mobs"};
		sightMask = LayerMask.GetMask(sightLayers);
		
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
			Vector2 rayDir = player.transform.position - this.transform.position;
			RaycastHit2D hit = Physics2D.Raycast(this.transform.position, rayDir, 1000, sightMask);
			
			if(hit && hit.transform.gameObject.tag == "Player") 
			{
				// Debug.Log ("Hit Transform" + hit.transform.position + " Player Transform " + player.transform.position);
				WalkTowards(player.transform.position); 
			}
			else
			{
				TrailCrumb crumbToFollow = null;
				
				foreach(TrailCrumb crumb in playerTrail.trail)
				// Get the first crumb that was dropped rather than ALL of the crumbs at once.
				// Do a raycast to the crumb
				// If it doesn't hit, look to the next crumb.
				// Condition: If none of the crumbs hit, go back to patrolling
				
				{
					rayDir = crumb.transform.position - this.transform.position;
					RaycastHit2D[] hitArray = Physics2D.RaycastAll(this.transform.position, rayDir, 1000, trackMask);
					
					for(int i = 0; i < hitArray.Length && hitArray[i].transform.tag == "Trail"; i++)
					{
						TrailCrumb hitCrumb = hitArray[i].transform.GetComponent<TrailCrumb>();
						
						if(crumbToFollow == null || crumbToFollow.GetLifeTime() > hitCrumb.GetLifeTime())
						{
							crumbToFollow = hitCrumb;
						}
					}
				}
				
				if(crumbToFollow != null)
				{
					WalkTowards(crumbToFollow.transform.position);
				}
				else
				{
					Debug.Log("Player Lost");
					//StopCoroutine("FollowPlayer");
					OnPlayerLost();
				}
			}
			
			yield return new WaitForFixedUpdate();
		}
	}
	
	IEnumerator Patrol()
	{
		state = EnemyState.PATROL;
		
		int i = ClosestWaypoint ();
		while(state == EnemyState.PATROL)
		{
			
			Vector2 to = patrolPath[i].transform.position;
			
			while( (Vector2)this.transform.position != to && state == EnemyState.PATROL )
			{
				WalkTowards(to);
				
				yield return null;
			}
			
			
			i = (i >= patrolPath.Length - 1)? 0 : ++i;
		}
	}
	
	private void WalkTowards(Vector2 to)
	{
		Vector2 direction = to - (Vector2)this.transform.position;
		this.transform.Translate(Vector3.ClampMagnitude(direction, speed*Time.deltaTime), Space.World);
		this.transform.right = to - (Vector2)this.transform.position;
	}

    public void face(Vector2 point)
    {
        this.transform.right = point - (Vector2)this.transform.position;
    }
       
	private int ClosestWaypoint()
	{
		int nearest = 0;
		for ( int i = 0; i<this.patrolPath.Length; i++)
		{
			float distance = (this.transform.position - this.patrolPath[i].transform.position).magnitude;
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
			Debug.Log("I am followin");
			StartCoroutine("FollowPlayer");
			StopCoroutine("Patrol");

		}
		
	}
	
	public void OnPlayerLost()
	{
		// if(state == EnemyState.CHASING) // Unnecessary
		{
			StartCoroutine("Patrol");
			StopCoroutine("FollowPlayer");
		}
	}

    public void GetHit(float damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            Die();
        }
    }

	public void Die()
	{

		this.sprite.enabled = false;
		this.minimapSprite.enabled = false;
		this.collider2D.enabled = false;
        this.LoSCollider.GetComponent<PolygonCollider2D>().enabled = false;
        state = EnemyState.DEAD;
	}

    public void BoostSight()
    {
        if (vision == EnemyVisionState.NORMAL)
        {
            vision = EnemyVisionState.BOOSTED;
            Debug.Log("BOOST!");
            LoSCollider.localScale = new Vector3(LoSCollider.localScale.x * 2, LoSCollider.localScale.y, LoSCollider.localScale.z);
        }
    }

    public void NormalSight()
    {
        if (vision == EnemyVisionState.BOOSTED)
        {
            vision = EnemyVisionState.NORMAL;
            Debug.Log("BOOST!");
            LoSCollider.localScale = new Vector3(LoSCollider.localScale.x / 2, LoSCollider.localScale.y, LoSCollider.localScale.z);
        }
    }
    
	void OnCollisionStay2D(Collision2D coll)
	{
		if(coll.transform.tag == "Wall")
		{
			Vector2 wallNormal = coll.contacts[0].normal;
			Vector2 wallParallel = new Vector2(wallNormal.y, -wallNormal.x);
			Vector2 aimDirection = this.transform.right * speed;
			Vector2 currentVelocity = Vector3.Project(aimDirection, wallParallel);
			
			float lostSpeed = speed - currentVelocity.magnitude;
			Vector2 lostVelocity = Vector3.Normalize(currentVelocity) * lostSpeed;
			this.transform.Translate(lostVelocity*Time.deltaTime, Space.World);
		}
	}
}

