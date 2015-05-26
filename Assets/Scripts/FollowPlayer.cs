using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	GameObject player;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void LateUpdate () {
        Vector3 currentPos = player.transform.position;
        this.transform.position = new Vector3(currentPos.x, currentPos.y, this.transform.position.z);
	}
}
