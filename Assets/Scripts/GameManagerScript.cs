using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManagerScript : MonoBehaviour {
	
	public Text timerText;
	public float timer = 0.0f;
	// Use this for initialization
	void Start () {
		//...wait really there's no Text initialization
	}

	// Update is called once per frame
	void Update () {
		//It's meant to tick up, not act as a time limit. And the timer should
		// be displayed onscreen at all times, not just during pause.
		// We'll need proper buttons to go to the Main Menu also (we shouldn't have
		// both P and Esc)
		timer += Time.deltaTime;
//		timer = Mathf.Max (0, timer);
		timerText.text = timer.ToString();
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