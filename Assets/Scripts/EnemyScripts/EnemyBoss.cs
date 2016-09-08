using UnityEngine;

public enum EnemyBossState
{
    PATROL,
    ATTACK,
    DEAD,
    CHASING,
    BLINDED,
    BOSSSPECIAL,
    WAITING,
    CHARGING,
}
public class EnemyBoss : Enemy
{
    // PUBLIC VARIABLES
    public EnemyBossState bossstate = EnemyBossState.PATROL;

    // PRIVATE VARIABLES

    private float[] triggerLevels = new float[] { 150, 300, 450 };
    private float timer = 0;
    public float waitDuration = 5;
    public AudioClip waitingClip;
    public AudioClip immuneClip;
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
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        switch (bossstate)
        {
            case EnemyBossState.PATROL:
                Patrol();
                break;
            case EnemyBossState.CHASING:
                FollowPlayer();
                break;
            case EnemyBossState.ATTACK:
                AttackPlayer();
                break;
            case EnemyBossState.BOSSSPECIAL:
                BossSpecial();
                break;
            case EnemyBossState.WAITING:
                Waiting();
                break;
            case EnemyBossState.CHARGING:
                Charge();
                break;
        }
    }

    public void reveal()
    {
        timer = 0;
        bossstate = EnemyBossState.CHASING;
        sprite.color = new Color(1f, 1f, 1f, 1f);
        sprite.enabled = true;
    }

    void Charge()
    {
        WalkTowards(player.transform.position);
        timer += Time.deltaTime;
        if (timer > waitDuration)
        {
            timer = 0;
            bossstate = EnemyBossState.BLINDED;
            sprite.color = new Color(1f, 1f, 1f, 1f);
            sprite.enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }
    void Waiting()
    {
        timer += Time.deltaTime;
        if (timer > waitDuration)
        {
            timer = 0;
            bossstate = EnemyBossState.CHARGING;
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }
    public void BossSpecial()
    {
        Vector2 teleportDirection = new Vector2(9001, 0);
        while (Physics2D.Raycast(transform.position, teleportDirection, teleportDirection.magnitude, sightMask).collider != null)
        {
            teleportDirection = Random.insideUnitCircle * 500;
        }
        transform.Translate(teleportDirection.x, teleportDirection.y, 0, Space.World);
        bossstate = EnemyBossState.WAITING;
        timer = 0;
        sprite.color = new Color(1f, 1f, 1f, 0f);
        GetComponent<AudioSource>().clip = waitingClip;
        GetComponent<AudioSource>().Play();
        GetComponent<CircleCollider2D>().enabled = false;
    }

    // WalkTowards: Tells enemy to move to a specified location.
    // TODO need to make this smarter using pathfinding or something D:
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

    public override void OnPlayerSighted()
    {
        if (bossstate != EnemyBossState.WAITING)
            GetComponent<CircleCollider2D>().enabled = true;
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

    override public void GetHit(float damage)
    {
        if (bossstate != EnemyBossState.CHARGING)
        {
            health = health - damage;
            if (health <= 0)
            {
                Die();
            }
            for (int i = 0; i < triggerLevels.Length; i++)
            {
                if (triggerLevels[i] > health)
                {
                    triggerLevels[i] = -1;
                    bossstate = EnemyBossState.BOSSSPECIAL;
                }
            }
            if (bossstate == EnemyBossState.PATROL || bossstate == EnemyBossState.BLINDED)
            {
                sprite.color = new Color(1f, 1f, 1f, 1f);
                sprite.enabled = true;
                GetComponent<CircleCollider2D>().enabled = true;

                bossstate = EnemyBossState.CHASING;
            }
        }
        else
        {
            GetComponent<AudioSource>().clip = immuneClip;
            GetComponent<AudioSource>().Play();
        }
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
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().takeHit(ATTACK_DAMAGE);
            sprite.color = new Color(1f, 1f, 1f, 1f);
            bossstate = EnemyBossState.CHASING;
            sprite.enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }

}

