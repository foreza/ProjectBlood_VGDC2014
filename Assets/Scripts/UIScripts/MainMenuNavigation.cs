using UnityEngine;
using System.Collections;

public class MainMenuNavigation : MonoBehaviour {

	public void LoadCanvas(GameObject callingCanvas, GameObject canvasToLoad) {
		callingCanvas.SetActive(false);
		canvasToLoad.SetActive(true);
	}
}
