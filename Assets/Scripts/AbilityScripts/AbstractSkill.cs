using UnityEngine;
using System.Collections;

public abstract class AbstractSkill : MonoBehaviour {
    private Transform wielder;
	// Use this for initialization
	void Start () {
        wielder = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public abstract void Use();

}
