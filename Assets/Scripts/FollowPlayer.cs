using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	GameObject player;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void LateUpdate () {
		transform.position = player.transform.position;
	}
}
