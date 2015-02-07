using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIMaster : MonoBehaviour {
	public GameObject oneObjective;
	List<Objective> descriptions;
	ArrayList children;
	public Font font1;
	GameObject theobj;

	void Start(){
		descriptions = new List<Objective> ();
		Enemy anenemy = GameObject.Find ("Enemy").GetComponent<Enemy> ();
		descriptions [0] = new Objective ("Kill Him", "He Needs To Die", "kill", null, anenemy);
	}

	void Update(){
		if(Input.GetKeyDown("tab")){
			objectives ();
		}
		if (Input.GetKeyUp ("tab")) {
			children = new ArrayList();
			foreach (Transform child in transform) children.Add(child.gameObject);
			foreach(GameObject child in children) {Destroy (child);}
		}
		foreach(Objective o in descriptions){
			if(o.isObjectiveDone()){
				descriptions.Remove (o); //hopefully this only removes o and not all objects of type o.
			}
		}
	}
	 
	void objectives(){
		RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();
		float screenwidth = containerRectTransform.rect.width;
		float screenheight = containerRectTransform.rect.height;
		float x = 350;
		float y = 0;
		foreach (Objective obj in descriptions) {
			theobj = Instantiate (oneObjective) as GameObject;
			theobj.transform.parent = gameObject.transform;
			RectTransform rectTransform = theobj.GetComponent<RectTransform> ();
			rectTransform.offsetMin = new Vector2 (x, y);
			Debug.Log ("LL:" + x + " " + y);
			rectTransform.offsetMax = new Vector2 (x + 60, y - 50);
			Debug.Log ("UR:" + (x + 60) + " " + (y));
			theobj.GetComponentInChildren<Text> ().text = obj.description;
			y -= 50;
				}

	}
	
}