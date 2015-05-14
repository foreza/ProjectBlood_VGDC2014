using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject pauseMenu, instructionsMenu, optionsMenu;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (!pauseMenu.activeSelf) {
				//if (instructionsMenu.activeSelf) {
				//	BackToPause();
				//}
				if (optionsMenu.activeSelf || instructionsMenu.activeSelf) {
					BackToPause();
				}
				else {
					PauseGame();
				}
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

	public void InstructionsMenu() {
		pauseMenu.SetActive(false);
		instructionsMenu.SetActive(true);
	}

	public void OptionsMenu() {
		pauseMenu.SetActive(false);
		optionsMenu.SetActive(true);
	}

	public void ResumeGame() {
		Time.timeScale = 1;
		pauseMenu.SetActive(false);
	}

	public void BackToPause() {
		pauseMenu.SetActive(true);
		instructionsMenu.SetActive(false);
		optionsMenu.SetActive(false);
	}
}
