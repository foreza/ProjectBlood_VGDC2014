using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIMaster : MonoBehaviour {
	public GameObject oneObjective;
	ArrayList children;
	GameObject theobj;
	public List<Objective> descriptions;
	public ScrollableList scrolist;
	Enemy anenemy;

	void Start(){
		descriptions = new List<Objective>();
		anenemy = GameObject.Find ("/Enemy").GetComponent<Enemy> (); //find whatever enemy is first in the scene
		//UnityEngine.Debug.Log (anenemy);
		descriptions.Add ( new Objective ("Kill Him", "He Needs To Die", "kill",anenemy, new Vector3(0,0,0)));
		scrolist = GetComponent<ScrollableList> ();
	}

	void Update(){
		foreach (Objective o in descriptions) {
			if(o.isObjectiveDone ()){
				descriptions.Remove(o);
			}
		}
	
		
		if(Input.GetKeyDown("tab")){
			scrolist.doTheGUI();
		}
		if (Input.GetKeyUp ("tab")) {
			/*
			children = new ArrayList ();
			foreach (Transform child in transform)
					children.Add (child.gameObject);
			foreach (GameObject child in children) {
					Destroy (child);

			}
			*/
		}
	}

	void objectives(){
		//foreach (Objective obj in descriptions) {
		//theobj.transform.parent = gameObject.transform;
		//}
			/*
		float x = 350;
		float y = 0;
		foreach (Objective obj in descriptions) {
			theobj = Instantiate (oneObjective) as GameObject;
			theobj.transform.parent = gameObject.transform;
			RectTransform rectTransform = theobj.GetComponent<RectTransform> ();
			rectTransform.offsetMin = new Vector2 (x, y);
			rectTransform.offsetMax = new Vector2 (x + 60, y - 50);
			theobj.GetComponentInChildren<Text> ().text = obj.description;
			y -= 50;

		}
	*/
	}
	
}