using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManagerScript : MonoBehaviour {
	public Text timerText;
	public float timer = 0.0f;
    private AudioSource music;
	// Use this for initialization
	void Start () {
        music = GetComponent<AudioSource>();
        music.ignoreListenerVolume = true;
        if (PlayerPrefs.HasKey(Options.SoundLevel)) // set volumes from stored values
            AudioListener.volume = PlayerPrefs.GetInt(Options.SoundLevel) / 100.0f;
        if (PlayerPrefs.HasKey(Options.MusicLevel)) // set volumes from stored values
            music.volume = PlayerPrefs.GetInt(Options.MusicLevel) / 100.0f;

        
		//...wait really there's no Text initialization
	}

	// Update is called once per frame
	void Update () {

		// Below is old code for the timer system. May use later.
//		//It's meant to tick up, not act as a time limit. And the timer should
//		// be displayed onscreen at all times, not just during pause.
//		// We'll need proper buttons to go to the Main Menu also (we shouldn't have
//		// both P and Esc)
//		timer += Time.deltaTime;
////		timer = Mathf.Max (0, timer);
//		timerText.text = timer.ToString();
//		if (Input.GetKeyDown (KeyCode.Escape)) {
//			Application.LoadLevel("MainMenu");
//		}
//		if (Input.GetKeyDown (KeyCode.P)) {
//			if (Time.timeScale == 0)
//				Time.timeScale = 1.0f;
//			else
//				Time.timeScale = 0;
//		}
	}
}