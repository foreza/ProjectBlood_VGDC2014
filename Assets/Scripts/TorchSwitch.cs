
using UnityEngine;
using System.Collections;

public class TorchSwitch : MonoBehaviour {
	
	public GameObject torch; // The torch this switch affects
	
	public bool canLight = true; // Can this switch light the torch?
	public bool canExtinguish = false; // Can this switch extinguish the torch?
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			this.audio.Play ();
			if((torch.GetComponent<TorchScript>().IsLit() && canExtinguish) || (!torch.GetComponent<TorchScript>().IsLit() && canLight))
				torch.GetComponent<TorchScript>().ToggleLight();
		}
	}
}
