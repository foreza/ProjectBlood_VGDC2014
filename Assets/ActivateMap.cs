using UnityEngine;
using System.Collections;

public class ActivateMap : MonoBehaviour {

	public Camera fullMapCamera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI () {
		if (Input.GetKey(KeyCode.M)) {
			fullMapCamera.gameObject.SetActive(true);
		} else {
			fullMapCamera.gameObject.SetActive(false);
		}
	}
}
