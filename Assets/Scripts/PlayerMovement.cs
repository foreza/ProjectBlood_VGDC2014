using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed = 150f;

	// Use this for initialization
	void Start () {
	
	}
	
	// FixedUpdate is called once per frame
	void FixedUpdate () {
		rigidbody2D.velocity = new Vector3 (Input.GetAxisRaw ("Horizontal") * speed, Input.GetAxisRaw ("Vertical") * speed, 0);
	}
}
