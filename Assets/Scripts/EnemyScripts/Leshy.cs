using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LeshyState
{
    PATROL,
    ATTACK,
    DEAD,
    BOSSSPECIAL,
    CHASING,
}
public class Leshy : Enemy
{
    // PUBLIC VARIABLES
    public LeshyState bossstate = LeshyState.PATROL;

    // PRIVATE VARIABLES

    private float[] triggerLevels = new float[] { 150, 300, 450 };
    public AudioClip waitingClip;
    public AudioClip immuneClip;
    public GameObject treeHeadPrefab;
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
    }

    void FixedUpdate()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (bossstate != LeshyState.DEAD)
        {
            if (bossstate == LeshyState.PATROL)
                Patrol();
            else if (bossstate == LeshyState.CHASING)
                FollowPlayer();
            else if (bossstate == LeshyState.ATTACK)
            {
                AttackPlayer();
            }
            else if (bossstate == LeshyState.BOSSSPECIAL)
            {
                BossSpecial();
            }
        }
    }
    public void BossSpecial()
    {
        print("BOSS");
        GameObject tree = Instantiate(treeHeadPrefab) as GameObject;
        
        bossstate = LeshyState.CHASING;
        this.GetComponent<AudioSource>().clip = this.waitingClip;
        this.GetComponent<AudioSource>().Play();
        this.GetComponent<CircleCollider2D>().enabled = false;
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

    public override void OnPlayerSighted()
    {
            this.GetComponent<CircleCollider2D>().enabled = true;
        //summon vines
            foreach (GameObject vine in GameObject.FindGameObjectsWithTag(Tags.vine))
            {
                vine.GetComponent<TreeHoming>().Summon();
            }
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
        for (int i = 0; i < triggerLevels.Length; i++)
        {
            if (triggerLevels[i] > health)
            {
                triggerLevels[i] = -1;
                bossstate = LeshyState.BOSSSPECIAL;
            }
        }
            if (bossstate == LeshyState.PATROL)
            {
                this.sprite.color = new Color(1f, 1f, 1f, 1f);
                bossstate = LeshyState.CHASING;
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
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().takeHit(ATTACK_DAMAGE);
            this.sprite.color = new Color(1f, 1f, 1f, 1f);
            bossstate = LeshyState.CHASING;
        }
    }

}

