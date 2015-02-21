using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTrail : MonoBehaviour 
{

	// PUBLIC VARIABLES
	public static float CRUMB_SPAWN_RATE = 0.2f;
	public static float CRUMB_LIFETIME = 1.8f;
    private Player player;
	// PRIVATE VARIABLES
	private static GameObject crumbObj;
	private float spawnTimer;
	private LinkedList <TrailCrumb> crumbTrail;

	void Start ()
	{
        player = GetComponent<Player>();
		crumbTrail = new LinkedList<TrailCrumb> ();					// list of crumb scripts. oldest crumb is at the end.
		crumbObj = Resources.Load ( "TrailObject" ) as GameObject;	// reference to TrailObject prefab.
		spawnTimer = 0.0f;											// timer to keep track of when to spawn a new crumb
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		if ( spawnTimer >= CRUMB_SPAWN_RATE && player.state != PlayerState.STEALTH)		// If the time to spawn a crumb has come ...
		{
			spawnTimer = 0;		// reset the timer.

			// instantiate a new crumb and set its position to the player's current position
			GameObject crumbInstance = Instantiate ( crumbObj ) as GameObject;
			crumbInstance.transform.position = this.transform.position;
			crumbInstance.layer = LayerMask.NameToLayer ( "Tracks" );

			// add the new crumb SCRIPT to the BEGINNING of the trail.
			crumbTrail.AddFirst ( crumbInstance.GetComponent <TrailCrumb> () );
		}
		else 			// Otherwise, add time to spawnTimer.
			spawnTimer += Time.deltaTime;

		// destroy the oldest crumb if its lifetime is expired
		if ( crumbTrail.Count > 0 && crumbTrail.Last.Value.GetLifeTime () > CRUMB_LIFETIME )
		{
			GameObject toDestroy = crumbTrail.Last.Value.gameObject;
			crumbTrail.RemoveLast ();
			Destroy ( toDestroy );
		}
	}

	// Getter for the crumb trail
	public LinkedList <TrailCrumb> GetCrumbTrail ()
	{
		return crumbTrail;
	}
}
