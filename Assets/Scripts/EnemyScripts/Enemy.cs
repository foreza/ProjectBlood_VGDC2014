using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyState
{
    PATROL,
    CHASING,
    ATTACK,
    DEAD,
    STATIONARY
}

enum EnemyVisionState
{
    NORMAL,
    BOOSTED
}

public class Enemy : Character
{
    // PUBLIC VARIABLES
    public GameObject[] patrolPath;
    public EnemyState state = EnemyState.PATROL;

    public float distanceToPlayer;

    // PRIVATE VARIABLES
    protected Player player;
    protected PlayerTrail playerTrail;
    protected SpriteRenderer sprite;
    protected Transform LoSCollider;
    protected LayerMask trackMask;
    protected LayerMask sightMask;
    protected int currWaypointIndex;
    protected ParticleSystem deathParticleEffect;
    protected static float ATTACK_COOLDOWN = 1.0f;
    protected float attackTimer = 0.0f;
    protected float ATTACK_DAMAGE = 25.0f;
    public float DISTANCE_TO_ATTACK = 50.0f;
    protected List<AbstractSkill> abilities;
    protected Weapon weapon;
    protected EnemyState startState;
    protected Vector3 startRotation;

    // INITIALIZE
    void Start()
    {
        sprite = transform.FindChild("EnemyPlaceholder").GetComponent<SpriteRenderer>();
        LoSCollider = transform.FindChild("LineOfSight");
        player = GameObject.Find("Player").GetComponent<Player>();
        playerTrail = player.GetComponent<PlayerTrail>();
        string[] trackLayers = { "LightWalls", "Tracks" };
        trackMask = LayerMask.GetMask(trackLayers);
        string[] sightLayers = { "LightWalls", "Mobs" };
        sightMask = LayerMask.GetMask(sightLayers);
        currWaypointIndex = ClosestWaypoint();
        deathParticleEffect = transform.FindChild("EnemyPlaceholder").GetComponent<ParticleSystem>();
        weapon = transform.FindChild("Sword").GetComponent<Sword>();

        abilities = new List<AbstractSkill>();
        AbstractSkill probAbility = transform.GetComponent<RandomTeleport>();
        if (probAbility != null)
        {
            abilities.Add(probAbility);
        }

        startState = state;
        startRotation = transform.eulerAngles;
    }

    // FIXED UPDATE
    void FixedUpdate()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (!LoSCollider.gameObject.activeSelf && !(player.state == PlayerState.STEALTH))
        {
            LoSCollider.gameObject.SetActive(true);
        }
        if (state != EnemyState.DEAD)
        {
            if (state == EnemyState.PATROL)
                Patrol();
            else if (state == EnemyState.CHASING)
                FollowPlayer();
            else if (state == EnemyState.ATTACK)
            {
                AttackPlayer();
            }
        }
    }

    // FollowPlayer: tries to find the player and move towards him. Otherwise, move towards the latest crumb.
    public void FollowPlayer()
    {
        // ray direction is towards the player
        Vector2 rayDir = player.transform.position - this.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, rayDir, 1000, sightMask);

        // In the case that the player stealths.
        if (player.state == PlayerState.STEALTH)
        {
            LoSCollider.gameObject.SetActive(false);
        }

        if (hit && hit.collider.gameObject.tag == "Player" && player.state != PlayerState.STEALTH)		// if the player is sighted, move towards him ...
        {
            WalkTowards(player.transform.position);

            if (distanceToPlayer <= DISTANCE_TO_ATTACK)
            {
                state = EnemyState.ATTACK;
            }

        }
        else 														// ... Otherwise, try to find the breadcrumbs.
        {
            Vector2 crumbToFollow = Vector2.zero;		// position to move towards

            // Starting from the newest crumb (first), check if visible. If it's visible move towards it.
            // Start at newest, otherwise it will try to follow the full trail like an idiot.
            foreach (TrailCrumb crumb in playerTrail.GetCrumbTrail())
            {
                // cast the ray towards the crumb
                rayDir = crumb.transform.position - this.transform.position;
                hit = Physics2D.Raycast(this.transform.position, rayDir, 1000, trackMask);

                // if it hits the crumb, move towards crumb
                if (crumbToFollow == Vector2.zero && hit && hit.transform.gameObject.tag == "Trail")
                {
                    crumbToFollow = hit.point;
                }
            }

            // If there is a crumb to follow, follow it. Otherwise, lose the player.
            if (crumbToFollow != Vector2.zero)
                WalkTowards(crumbToFollow);
            else
            {
                state = EnemyState.PATROL;
                currWaypointIndex = ClosestWaypoint();
            }
        }
    }

    // AttackPlayer: attack player if within attack radius
    public void AttackPlayer()
    {
        if (distanceToPlayer > DISTANCE_TO_ATTACK)
        {
            state = EnemyState.CHASING;
        }
        else if (player.state == PlayerState.STEALTH)
        {
            state = EnemyState.PATROL;

        }
        else if (attackTimer == 0)
        {
            // player.GetComponent<Character>().health -= ATTACK_DAMAGE; // deals damage to player

            weapon.Attack();
            attackTimer += Time.deltaTime;
            //LookTowards(player.transform.position); //face the player

        }
        else if (attackTimer > 0)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= ATTACK_COOLDOWN)
            {
                attackTimer = 0;
            }
        }
    }

    // Patrol: if state is patrolling, do patrol
    public void Patrol()
    {
        if (patrolPath.Length > 0)
        {
            Vector2 target = patrolPath[currWaypointIndex].transform.position;		// get current waypoint's position.
            if (Vector2.Distance((Vector2)this.transform.position, target) > .05)
            {						// if not at the waypoint, move towards it ~~ !
                WalkTowards(target);
            }
            else 																	// otherwise, set current waypoint to the next one.
            {
                if (patrolPath.Length == 1)
                {
                    transform.eulerAngles = startRotation;
                    state = EnemyState.STATIONARY;
                }
                else
                {
                    currWaypointIndex = (currWaypointIndex + 1) % patrolPath.Length;
                }
            }
        }
    }

    // WalkTowards: Tells enemy to move to a specified location.
    // TODO need to make this smarter using pathfinding or something D:
    private void WalkTowards(Vector2 to)
    {
        Vector2 direction = to - (Vector2)this.transform.position;
        this.transform.Translate(Vector3.ClampMagnitude(direction, speed * Time.deltaTime), Space.World);
        this.transform.right = to - (Vector2)this.transform.position;
    }

    private void LookTowards(Vector2 to)
    {
        this.transform.right = to - (Vector2)this.transform.position;
    }

    public virtual void OnPlayerSighted()
    {
        this.state = EnemyState.CHASING;
        this.LoSCollider.GetComponent<AudioSource>().Play();
    }

    private int ClosestWaypoint()
    {
        int nearest = 0;
        for (int i = 0; i < this.patrolPath.Length; i++)
        {
            float distance = (this.transform.position - this.patrolPath[i].transform.position).magnitude;
            if (distance < (this.transform.position - this.patrolPath[nearest].transform.position).magnitude)
            {
                nearest = i;
            }
        }
        return nearest;
    }

    virtual public void GetHit(float damage)
    {
        health = health - damage;
        state = EnemyState.CHASING;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        deathParticleEffect.Play(); // Temporarily removed since it was throwing errors. TODO : FIX THIS D:
        this.sprite.enabled = false;
        this.GetComponent<Collider2D>().enabled = false;
        this.LoSCollider.GetComponent<PolygonCollider2D>().enabled = false;
        state = EnemyState.DEAD;
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.transform.tag == "Wall")
        {
            Vector2 wallNormal = coll.contacts[0].normal;
            Vector2 wallParallel = new Vector2(wallNormal.y, -wallNormal.x);
            Vector2 aimDirection = this.transform.right * speed;
            Vector2 currentVelocity = Vector3.Project(aimDirection, wallParallel);

            float lostSpeed = speed - currentVelocity.magnitude;
            Vector2 lostVelocity = Vector3.Normalize(currentVelocity) * lostSpeed;
            this.transform.Translate(lostVelocity * Time.deltaTime, Space.World);
        }
    }
}

