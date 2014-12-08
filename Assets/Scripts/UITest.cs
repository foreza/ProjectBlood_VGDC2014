using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITest : MonoBehaviour {
	public Slider healthbarslider;
	public Slider energybarslider;
	public GameObject oneObjective;
	public string[] descriptions;
	ArrayList children;
	//If the objective is done, remove its description from the list. How should this link in to the backend?
	public Font font1;
	GameObject theobj;

	void setBar(Slider bar,float amount){
		bar.value = amount;
		}

	void Update(){
		if(Input.GetKeyDown("tab")){
			objectives ();
		}
		if (Input.GetKeyUp ("tab")) {
			Debug.Log ("OnKeyUp called");
			children = new ArrayList();
			foreach (Transform child in transform) children.Add(child.gameObject);
			foreach(GameObject child in children) {Destroy (child);}
		}
		//change to be set to player's max health later
		setBar (healthbarslider, GameObject.Find ("Player").GetComponent<Player>().health/100);
		setBar (energybarslider, GameObject.Find ("Player").GetComponent<Player>().energy/50);
	}
	 
	void objectives(){
		descriptions = new string[4] {"one","two","three","77"};
		RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();
		float screenwidth = containerRectTransform.rect.width;
		float screenheight = containerRectTransform.rect.height;
		float x = 350;
		float y = 0;
		Debug.Log ("Just before foreach");
		foreach (string obj in descriptions) {
			theobj = Instantiate (oneObjective) as GameObject;
			theobj.transform.parent = gameObject.transform;
			RectTransform rectTransform = theobj.GetComponent<RectTransform> ();
			rectTransform.offsetMin = new Vector2 (x, y);
			Debug.Log ("LL:" + x + " " + y);
			rectTransform.offsetMax = new Vector2 (x + 60, y - 50);
			Debug.Log ("UR:" + (x + 60) + " " + (y));
			theobj.GetComponentInChildren<Text> ().text = obj;
			y -= 50;
				}

	}
	
}
