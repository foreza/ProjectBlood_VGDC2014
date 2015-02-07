using UnityEngine;
using System.Collections;
using System;
public class Objective
{
	public string name;
	public string description;
	public string type; //"kill" for kill people, "go" for go to location. Did this to avoid making a class for each.
	public Vector3 coords; //Set if it is a goto objective.
	public Enemy evil; //set if a kill objective.
	public UIMaster thegui;
	private Player player;

	public Objective(string objname, string objdesc, string objtype, Vector3 objcoords, Enemy evil)
	{
		name = objname;
		description = objdesc;
		type = objtype;
		thegui = GameObject.Find ("Main Camera").GetComponent<UIMaster> ();
		player = GameObject.Find ("Player").GetComponent<Player> ();
	}
	//checks if objective is done and automatically removes if it is.
	bool isObjectiveDone(string objType)
	{
		if (objType == "kill") {
			if (evil.health <= 0) {
					//remove objective from list in UITest
					return true;
			} else {
					return false;
			}
		} else if (objType == "go") {
			Vector3 offset = player.transform.position - coords;
			float sqrLen = offset.sqrMagnitude;
			if (sqrLen < 10 * 10) {
					//Remove from objective list in UITest
					return true;
			} else {
					return false;
			}
		} else {
			throw new Exception("Type must be 'kill' or 'go'");
		}
	}
}
