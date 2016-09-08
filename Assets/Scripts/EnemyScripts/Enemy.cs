using UnityEngine;

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

        switch(state)
        {
            case EnemyState.PATROL:
                Patrol();
                break;
            case EnemyState.CHASING:
                FollowPlayer();
                break;
            case EnemyState.ATTACK:
                AttackPlayer();
                break;
        }
    }

    // FollowPlayer: tries to find the player and move towards him. Otherwise, move towards the latest crumb.
    public void FollowPlayer()
    {
        // ray direction is towards the player
        Vector2 rayDir = player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, 1000, sightMask);

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
                rayDir = crumb.transform.position - transform.position;
                hit = Physics2D.Raycast(transform.position, rayDir, 1000, trackMask);

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
            weapon.Attack();
            attackTimer += Time.deltaTime;
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
            if (Vector2.Distance(transform.position, target) > .05)
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
    // TODO need to make this smarter using pathfinding or something
    private void WalkTowards(Vector2 to)
    {
        Vector2 direction = to - (Vector2)transform.position;
        transform.Translate(Vector3.ClampMagnitude(direction, speed * Time.deltaTime), Space.World);
        transform.right = to - (Vector2)transform.position;
    }

    private void LookTowards(Vector2 to)
    {
        transform.right = to - (Vector2)transform.position;
    }

    public virtual void OnPlayerSighted()
    {
        state = EnemyState.CHASING;
        LoSCollider.GetComponent<AudioSource>().Play();
    }

    private int ClosestWaypoint()
    {
        int nearest = 0;
        for (int i = 0; i < patrolPath.Length; i++)
        {
            float distance = (transform.position - patrolPath[i].transform.position).magnitude;
            if (distance < (transform.position - patrolPath[nearest].transform.position).magnitude)
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
        sprite.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        LoSCollider.GetComponent<PolygonCollider2D>().enabled = false;
        state = EnemyState.DEAD;
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.transform.tag == "Wall")
        {
            Vector2 wallNormal = coll.contacts[0].normal;
            Vector2 wallParallel = new Vector2(wallNormal.y, -wallNormal.x);
            Vector2 aimDirection = transform.right * speed;
            Vector2 currentVelocity = Vector3.Project(aimDirection, wallParallel);

            float lostSpeed = speed - currentVelocity.magnitude;
            Vector2 lostVelocity = Vector3.Normalize(currentVelocity) * lostSpeed;
            transform.Translate(lostVelocity * Time.deltaTime, Space.World);
        }
    }
}

