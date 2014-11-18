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
	void OnGUI(){
		if(Input.GetKeyDown("tab")){
			objectives ();
		}
	}
	public Text obj1;
	public Font font1;
	void objectives(){

		
		RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();
		float width = containerRectTransform.rect.width;
		float height = containerRectTransform.rect.height;
		GameObject theobj = Instantiate (oneObjective) as GameObject;
		theobj.transform.parent = gameObject.transform;
		RectTransform rectTransform = theobj.GetComponent<RectTransform>();
	
		float x = 350;
		float y = -100;
		rectTransform.offsetMin = new Vector2(x, y);
		
		x = rectTransform.offsetMin.x + width;
		y = rectTransform.offsetMin.y + height;
		rectTransform.offsetMax = new Vector2(x, y);

		obj1 = gameObject.AddComponent<Text> ();
		obj1.text = "ASDFASDF";
		obj1.font = font1;


	}
	
}
