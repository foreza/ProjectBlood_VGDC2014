using UnityEngine;
using System.Collections;

public class TreeHoming : MonoBehaviour {
    private Player player;
    private Rigidbody2D rigid;
    public float acceleration = 20000.0f;
    public float duration = 50.0f;
    public float damage = 25.0f;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
        rigid = this.GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, duration);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rigid.AddForce(Time.deltaTime * acceleration * ((Vector2)(player.transform.position - this.transform.position)).normalized); //accelerate to the player
        if (rigid.velocity.magnitude < 1)
        {
            rigid.velocity = rigid.velocity.normalized;
        }
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().takeHit(damage); // We should take damage from the enemy.
        }

    }
}
