using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	private GameObject player;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void LateUpdate () {
        Vector3 playerPosition = player.transform.position;

        // Only update x and y positions
        transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
	}

}
