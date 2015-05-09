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
	public bool alreadyInstantiated;
	public GameObject nextLevelButton;

	void Start(){
		nextLevelButton = GameObject.Find ("NextLevelButton");
		nextLevelButton.SetActive (false);
		alreadyInstantiated = false;
		descriptions = new List<Objective>();
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		//Enemy anenemy = GameObject.Find ("/Boss").GetComponent<Enemy> (); //find whatever enemy is first in the scene
		//UnityEngine.Debug.Log (anenemy);
		foreach (GameObject enemy in enemies) {
						descriptions.Add (new Objective ("Kill Him", "He Needs To Die", "kill", enemy.GetComponent<Enemy>(), new Vector3 (0, 0, 0)));
				}
		scrolist = this.GetComponent<ScrollableList> ();
	}

	public void loadLevel2(){
		//Instantiate button
		Application.LoadLevel ("Organized_Build_Level2");
		}

	void Update(){
				System.Diagnostics.Debug.Assert (descriptions != null);
				
	
		

		if (Input.GetKeyDown ("tab")) {
			scrolist.doTheGUI ();
		}
				
		if (Input.GetKeyUp ("tab")) {
			children = new ArrayList ();
			foreach (Transform child in transform)
				if(!child.name.Contains("Slider"))
					if(!child.name.Contains ("Level"))
						children.Add (child.gameObject);
			foreach (GameObject child in children) {
					Destroy (child);
			}
		}
//        foreach (Objective o in descriptions)
//        {
//            if (o.isObjectiveDone())
//            {
//                descriptions.Remove(o);
//                break;
//            }
//            //UnityEngine.Debug.Log ("Enemy is "+ o.evil.distanceToPlayer);
//        }

		if (GameObject.FindGameObjectWithTag ("Boss"))
		    if (GameObject.FindGameObjectWithTag("Boss").GetComponent<Enemy>().state == EnemyState.DEAD && alreadyInstantiated == false)
		    {
		        //buttonTransform.anchoredPosition.x = 3; buttonTransform.anchoredPosition.y = -28;
		        nextLevelButton.SetActive(true);
		        alreadyInstantiated = true;
		    }
	}
	
}