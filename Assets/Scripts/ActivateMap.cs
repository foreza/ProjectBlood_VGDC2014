using UnityEngine;
using System.Collections;

public class ActivateMap : MonoBehaviour {

	public Camera fullMapCamera;
    bool cameraState;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // SetActive throws !IsActive && !GetRunInEditMode error if called within OnGui()
        fullMapCamera.gameObject.SetActive(cameraState);
	}

	void OnGUI () {
		if (Input.GetKey(KeyCode.M)) {
			cameraState = true;
		} else {
			cameraState = false;
		}
	}
}
