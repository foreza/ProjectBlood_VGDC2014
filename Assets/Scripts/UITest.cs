using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITest : MonoBehaviour {
	public Slider healthbarslider;
	public Slider energybarslider;
	void setBar(Slider bar,float amount){
		bar.value = amount;
		}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
