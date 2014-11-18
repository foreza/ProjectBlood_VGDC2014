using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManagerScript : MonoBehaviour {
	
	public Text timerText;
	public float timer = 60;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		timer = Mathf.Max (0, timer);
		timerText.text = (timer).ToString();
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevel("MainMenu");
		}
		if (Input.GetKeyDown (KeyCode.P)) {
			if (Time.timeScale == 0)
				Time.timeScale = 1.0f;
			else
				Time.timeScale = 0;
		}
	}
}