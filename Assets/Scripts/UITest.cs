using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITest : MonoBehaviour {
	public Slider healthbarslider;
	public Slider energybarslider;
	public GameObject objectivesPanel;
	public GameObject oneObjective;
	float x = 350;
	float y = -100;

	void setBar(Slider bar,float amount){
		bar.value = amount;
		}

	void OnGUI(){
		if(Input.GetKeyDown("tab")){
			objectives ();
		}
	}
	
	public Font font1;
	void objectives(){
		RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();
		float screenwidth = containerRectTransform.rect.width;
		float screenheight = containerRectTransform.rect.height;

		for (int i=0; i<10; i++) {
			GameObject cloneObj = Instantiate (oneObjective) as GameObject;
			cloneObj.transform.parent = gameObject.transform;
			RectTransform rectTransform = cloneObj.GetComponent<RectTransform> ();
				//width and height must be set if you want to see anything
			rectTransform.offsetMin = new Vector2 (x, y);

			rectTransform.offsetMax = new Vector2 (x+60, y+60);
			y -=20;
		}
	}
	
}
