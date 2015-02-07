﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
	private Player theplayer;
	private Slider bar;
	void Start () {
		theplayer = GameObject.Find ("Player").GetComponent<Player> ();
		bar = this.GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		bar.value = theplayer.health / theplayer.healthMax;
	}
}
