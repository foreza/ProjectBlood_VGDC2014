using UnityEngine;
using System.Collections;

public class TreeHoming : MonoBehaviour {
    private Player player;
    private Rigidbody2D rigid;
    public float acceleration = 20000.0f;
    public float damage = 25.0f;
    private GameObject leshy;
    private bool summoned = false;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
        leshy = GameObject.Find("Leshy");
        rigid = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (summoned)
        {
            if ((leshy.transform.position - this.transform.position).magnitude < 5)
            {
                Destroy(this.gameObject);
            }
            else
            {
                rigid.AddForce(Time.deltaTime * acceleration * ((Vector2)(leshy.transform.position - this.transform.position)).normalized); //gets pulled to leshy
            }
            
        }
        else
        {
            rigid.AddForce(Time.deltaTime * acceleration * ((Vector2)(player.transform.position - this.transform.position)).normalized); //accelerate to the player
            if (rigid.velocity.magnitude < 1)
            {
                rigid.velocity = rigid.velocity.normalized;
            }
        }
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().takeHit(damage); // We should take damage from the enemy.
        }

    }

    public void Summon()
    {
        summoned = true;
        //disable collision
        foreach (Collider2D c in GetComponentsInChildren<Collider2D>())
        {
            c.enabled = false;
     } 
    }
}
