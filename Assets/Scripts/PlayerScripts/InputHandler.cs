using UnityEngine;
using System.Collections;

//I was going to call this InputManager but decided it would be conbused with Unity's InputManger
public class InputHandler : MonoBehaviour 
{
	PlayerController controller;

	void Start ()
	{
		if(controller == null)
		{
			controller = GameObject.Find("Player").GetComponent<PlayerController>();
		}
	}
	
	//Update any objects with input here
	void Update () 
	{
		controller.UpdateController();
	}
}
