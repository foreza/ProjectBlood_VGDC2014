using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour {
    
    private AudioSource music;

	void Start () {
        music = GetComponent<AudioSource>();
        music.ignoreListenerVolume = true;

        // set volumes from stored values
        if (PlayerPrefs.HasKey(Options.SoundLevel)) {
            AudioListener.volume = PlayerPrefs.GetInt(Options.SoundLevel) / 100.0f;
        }
        if (PlayerPrefs.HasKey(Options.MusicLevel)) {
            music.volume = PlayerPrefs.GetInt(Options.MusicLevel) / 100.0f;
        }
	}

	void Update () {
		AudioListener.volume = PlayerPrefs.GetInt(Options.SoundLevel) / 100.0f;
		music.volume = PlayerPrefs.GetInt(Options.MusicLevel) / 100.0f;
	}

}