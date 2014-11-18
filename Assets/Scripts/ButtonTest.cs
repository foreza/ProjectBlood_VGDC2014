using UnityEngine;
using System.Collections;

public class ButtonTest : MonoBehaviour {
	
	public void buttonClick(){
		Debug.Log ("Button Clicked");
		Application.LoadLevel(1);
	}
	public void startLevel(){
		Application.LoadLevel(1);
	}
	public void exitGame(){
			Application.Quit();
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
