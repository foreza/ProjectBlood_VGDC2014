using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarNode : MonoBehaviour
{
	public Dictionary <AStarNode, Vector2> neighborDict;	// Make sure all POSSIBLE neighbors are defined. Vector2.x stores cost, Vector2.y doesn't matter.

	private Transform t;
	private int obstacleLayer;

	// INITIALIZE
	void Start ()
	{
		if ( neighborDict == null )	// avoid null reference exception
			neighborDict = new Dictionary <AStarNode, Vector2> ();
		t = GetComponent<Transform> ();		// automatically find Transform of game object
		obstacleLayer = LayerMask.GetMask ( new string[] { "LightWalls" } );
	}

	// FIXED UPDATE
	void FixedUpdate ()
	{
		// The below will check for obstructions to neighbors. If there's an obstruction, make the edge cost prohibitive.
		foreach ( AStarNode n in neighborDict.Keys )
		{
			Vector2 dir = n.GetPos () - (Vector2) t.position;
			RaycastHit2D hit = Physics2D.Raycast ( t.position, dir, dir.magnitude, obstacleLayer );
			if ( hit.collider != null )
				neighborDict [n] = new Vector2 ( -1, neighborDict [n].x );		// TODO using -1 for cost prohibitive edges for now
			else
				neighborDict [n] = new Vector2 ( neighborDict [n].y, neighborDict [n].y );		// otherwise, swap back in the original cost
		}
	}

	// Position Getter
	public Vector2 GetPos ()
	{
		return t.position;
	}
}
