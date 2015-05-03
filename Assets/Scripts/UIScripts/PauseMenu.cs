using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject pauseMenu, instructionsMenu, optionsMenu;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (!pauseMenu.activeSelf) {
				PauseGame();
			}
			else {
				ResumeGame();
			}
		}
	}

	public void PauseGame() {
		Time.timeScale = 0;
		pauseMenu.SetActive(true);
	}

	public void OptionsMenu() {
		
	}
	
	public void InstructionsMenu() {
		
	}

	public void ResumeGame() {
		Time.timeScale = 1;
		pauseMenu.SetActive(false);
	}
}
