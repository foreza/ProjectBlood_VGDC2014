using UnityEngine;
using System.Collections;

// Attach this script to game objects that should be absent in web player
public class WebDisable : MonoBehaviour {

	void Start () {
        if (Application.platform == RuntimePlatform.WindowsWebPlayer ||
            Application.platform == RuntimePlatform.OSXWebPlayer ||
            Application.platform == RuntimePlatform.WebGLPlayer) {
            gameObject.SetActive(false);
        }
	}
	
}
