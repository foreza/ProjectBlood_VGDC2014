using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	private GameObject player;

	void Start () 
	{
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 currentPos = player.transform.position;
		this.transform.position = new Vector3 (currentPos.x, currentPos.y, this.transform.position.z);
	}
}
