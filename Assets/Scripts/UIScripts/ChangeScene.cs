using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {
    public string mainSceneName;
	// Use this for initialization
	public void LoadScene (string sceneName = "") {
        if (sceneName == "")
            sceneName = mainSceneName;
		Application.LoadLevel (sceneName);
	}

	public void exitGame (){
		Application.Quit ();
	}
}
