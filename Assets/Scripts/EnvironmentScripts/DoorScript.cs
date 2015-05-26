using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

	public bool open = false; // Is the door open?

	private Vector3 openPosition; // The position of the door when it's open
	private Vector3 closedPosition; // The position of the door when it's closed

	private float totalMoveDistance; // The distance between the open position and closed position

	private bool doorIsTriggered; // Is the door currently moving?

	private float startTime; // The time at which the door started moving

	void Start () {
        totalMoveDistance = transform.lossyScale.x * GetComponent<BoxCollider2D>().size.x;
        if (open) {
            openPosition = transform.position;
            if (transform.rotation.z != 0) {
                closedPosition = openPosition + new Vector3(0, totalMoveDistance, 0);
            }
            else {
                closedPosition = openPosition + new Vector3(totalMoveDistance, 0, 0);
            }
		} else {
			closedPosition = transform.position;
			if (transform.rotation.z != 0) {
				openPosition = closedPosition + new Vector3(0, totalMoveDistance, 0);
            }
            else {
				openPosition = closedPosition + new Vector3(totalMoveDistance, 0, 0);
            }
		}
	}
	
	void FixedUpdate () {
		if (doorIsTriggered) {
			MoveDoor ();
		}
	}

	public void ToggleDoor() {
		open = !open;
		doorIsTriggered = true;
		startTime = Time.time;
		GetComponent<AudioSource>().Play ();
	}

	void MoveDoor() {
		float distCovered = (Time.time - startTime) * 20.0f;
		float fracJourney = distCovered / totalMoveDistance;

		if (open) {
			transform.position = Vector3.Lerp(closedPosition, openPosition, fracJourney);
			if (transform.position == openPosition) {
				doorIsTriggered = false;
			}
		} else {
			transform.position = Vector3.Lerp(openPosition, closedPosition, fracJourney);
			if (transform.position == closedPosition) {
				doorIsTriggered = false;
			}
		}
	}
	public bool IsOpen()
	{
		return open;
	}
}
