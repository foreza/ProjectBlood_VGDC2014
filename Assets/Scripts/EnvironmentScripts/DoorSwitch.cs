using UnityEngine;
using System.Collections;

public class DoorSwitch : MonoBehaviour {

	public GameObject door; // The gate this switch affects
	
	public bool canOpen = true; // Can this switch open the gate?
	public bool canClose = false; // Can this switch close the gate?

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			GetComponent<AudioSource>().Play ();
			if((door.GetComponent<DoorScript>().IsOpen() && canClose) || (!door.GetComponent<DoorScript>().IsOpen() && canOpen))
				door.GetComponent<DoorScript>().ToggleDoor();
		}
	}
}
