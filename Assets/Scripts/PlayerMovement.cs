using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed = 0.5f;
	private bool movingDiagonally = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") != 0) {
			transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * speed/2, Input.GetAxisRaw("Vertical") * speed/2, 0));
		} else {
			transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed, 0));
		}
	}
}
