using UnityEngine;
using System.Collections;

public struct Objective{
	public string description;
	public bool done;
}

public class UIObjectives : MonoBehaviour {
	public Objective[] levelobjectives = new Objective[10];
	public Color grayed;
	public Color full;
	System.Random rand;
	void Start(){
		rand = new System.Random ();
		grayed = new Color (1.0f, 1.0f, 1.0f, 0.2f);
		full = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		for(int x=0;x<10;x++) {
			levelobjectives[x].description = "test";
			levelobjectives[x].done = System.Convert.ToBoolean(x % 2);
				}
		}

	void OnGUI(){
		if(Input.GetKey ("tab")){
			GUI.BeginGroup (new Rect(Screen.width/4,Screen.height/4,Screen.width/2,Screen.height/2));
			  GUI.Box(new Rect(0,0,Screen.width/2,Screen.height/2), "Objectives");
			  GUI.BeginGroup(new Rect(0,20,Screen.width/2,Screen.width/2));
			    for(int x=0;x<10;x++){
				if(levelobjectives[x].done){GUI.color = grayed;} else {GUI.color = full;}
					GUI.Box (new Rect(0,x*20,Screen.width/4,20),levelobjectives[x].description); //these values were arrived at by trial and error		
				}
			  GUI.EndGroup ();
			GUI.EndGroup ();
		}
	}
}
	
