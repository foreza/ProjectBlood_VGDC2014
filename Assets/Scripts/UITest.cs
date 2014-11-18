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

	void objectives(){
		GameObject theobj = Instantiate (oneObjective) as GameObject;
		theobj.transform.parent = gameObject.transform;
		RectTransform theobjtransform = theobj.GetComponent<RectTransform> ();
		theobjtransform.offsetMax = new Vector2 (0, 0);
		theobjtransform.offsetMin = new Vector2 (0, 0);
	}
	
}
