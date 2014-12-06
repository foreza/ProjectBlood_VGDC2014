using UnityEngine;
using System.Collections;

public class ButtonTest : MonoBehaviour {
	
	public void buttonClick(){
		Debug.Log ("Button Clicked");
		Application.LoadLevel(2);
	}
	public void startLevel(){
		Application.LoadLevel(2);
	}
	public void exitGame(){
			Application.Quit();
	}

	public void loadTitleScreen(){

		Application.LoadLevel(0);
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
