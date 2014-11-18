using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITest : MonoBehaviour {
	public Slider healthbarslider;
	public Slider energybarslider;
	public GameObject objectivesPanel;
	public GameObject oneObjective;

	void setBar(Slider bar,float amount){
		bar.value = amount;
		}

	void Update(){
		if(Input.GetKeyDown("tab")){
			objectives ();
		}
	}
	
	public Font font1;
	GameObject cloneObj;
	void objectives(){
		Debug.Log ("Objectives called");
		RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();
		float screenwidth = containerRectTransform.rect.width;
		float screenheight = containerRectTransform.rect.height;
		float x = 350;
		float y = -100;
		for (int i=0; i<10; i++) {
			Debug.Log ("In loop number,"+i);
			cloneObj = Instantiate (oneObjective) as GameObject;
			cloneObj.transform.parent = gameObject.transform;
			RectTransform rectTransform = cloneObj.GetComponent<RectTransform> ();
			rectTransform.offsetMin = new Vector2 (x, y);
			rectTransform.offsetMax = new Vector2 (x+60, y+5);
			y -=20;
		}
	}
	
}
