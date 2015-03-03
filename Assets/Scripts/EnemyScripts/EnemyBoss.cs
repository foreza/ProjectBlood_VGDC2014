using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyBossState
{
    PATROL,
    CHASING,
    ATTACK,
    DEAD,
    STATIONARY,
    BOSSSPECIAL,
    WAITING,
    CHARGING,
    BLINDED,
}
public class EnemyBoss : Enemy
{
    // PUBLIC VARIABLES
    public EnemyBossState bossstate = EnemyBossState.PATROL;

    // PRIVATE VARIABLES

    private float[] triggerLevels = new float[]{150, 300, 450};

    void Start()
    {
        sprite = transform.FindChild("EnemyPlaceholder").GetComponent<SpriteRenderer>();
        minimapSprite = transform.FindChild("Minimap EnemyPlaceholder").GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").GetComponent<Player>();
        playerTrail = player.GetComponent<PlayerTrail>();
        string[] trackLayers = { "LightWalls", "Tracks" };
        trackMask = LayerMask.GetMask(trackLayers);
        string[] sightLayers = { "LightWalls", "Mobs" };
        sightMask = LayerMask.GetMask(sightLayers);
    }

    void FixedUpdate()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        rigidbody2D.velocity = Vector2.zero;
        if (bossstate != EnemyBossState.DEAD)
        {
            if (bossstate == EnemyBossState.PATROL)
                Patrol();
            else if (bossstate == EnemyBossState.CHASING)
                FollowPlayer();
            else if (bossstate == EnemyBossState.ATTACK)
            {
                AttackPlayer();
            }
            else if (bossstate == EnemyBossState.BOSSSPECIAL)
            {
                BossSpecial();
            }
        }
    }

    public void BossSpecial()
    {
        Vector2 teleportDirection = new Vector2(9001, 0);
        while (Physics2D.Raycast(this.transform.position, teleportDirection, teleportDirection.magnitude, sightMask).collider != null)
        {
            teleportDirection = Random.insideUnitCircle * 500;
        }
        this.transform.Translate(teleportDirection.x, teleportDirection.y, 0, Space.World);
        bossstate = EnemyBossState.WAITING;
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

    new public void OnPlayerSighted()
    {

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
        //Debug.Log (nearest);
        return nearest;
    }

    override public void GetHit(float damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            Die();
        }
        for(int i=0; i < triggerLevels.Length; i++)
        {
            if (triggerLevels[i] > health)
            {
                triggerLevels[i] = -1;
                bossstate = EnemyBossState.BOSSSPECIAL;
                Debug.Log("Bosstime!");
            }
        }
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

