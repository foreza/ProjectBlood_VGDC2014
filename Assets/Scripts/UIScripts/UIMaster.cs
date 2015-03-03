using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIMaster : MonoBehaviour {
	ArrayList children;
	GameObject theobj;
	public List<Objective> descriptions;
	public ScrollableList scrolist;

	void Start(){
		descriptions = new List<Objective>();
		Enemy anenemy = GameObject.Find ("/Enemy").GetComponent<Enemy> (); //find whatever enemy is first in the scene
		UnityEngine.Debug.Log (anenemy);
		descriptions.Add ( new Objective ("Kill Him", "He Needs To Die", "kill",anenemy, new Vector3(0,0,0)));
		scrolist = this.GetComponent<ScrollableList> ();
	}

	void Update(){
				System.Diagnostics.Debug.Assert (descriptions != null);
				foreach (Objective o in descriptions) {
						if (o.isObjectiveDone ()) {
								descriptions.Remove (o);
								break;
						}
						//UnityEngine.Debug.Log ("Enemy is "+ o.evil.distanceToPlayer);
				}
	
		
		if (Input.GetKeyDown ("tab")) {
			scrolist.doTheGUI ();
		}
				
		if (Input.GetKeyUp ("tab")) {
			children = new ArrayList ();
			foreach (Transform child in transform)
					children.Add (child.gameObject);
			foreach (GameObject child in children) {
					Destroy (child);
			}
		}
	}
	
}