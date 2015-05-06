using UnityEngine;
using System.Collections;

public class TreeHoming : MonoBehaviour {
    private Player player;
    private Rigidbody2D rigid;
    public float acceleration = 20000.0f;
    public float duration = 50.0f;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
        rigid = this.GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, duration);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rigid.AddForce(Time.deltaTime * acceleration * ((Vector2)(player.transform.position - this.transform.position)).normalized); //accelerate to the player
	}
}
