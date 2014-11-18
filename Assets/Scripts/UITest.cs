using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITest : MonoBehaviour {
	public Slider healthbarslider;
	public Slider energybarslider;
	public GameObject objectivesPanel;

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
		obj1 = gameObject.AddComponent<Text> ();
		obj1.text = "ASDFASDF";
		obj1.font = font1;

	}
	
}
