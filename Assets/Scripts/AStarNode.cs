using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarNode : MonoBehaviour
{
	public AStarNode[] neighbors;	// Make sure all POSSIBLE neighbors are defined. Vector2.x stores cost, Vector2.y doesn't matter.
	public string[] obstacleLayers = new string[] { "LightWalls" };
	public float cost = 1f;
	
	private float originalCost;
	private HashSet <int> obstacleLayerInts;
	
	// INITIALIZE
	void Start ()
	{
		foreach ( string layer in obstacleLayers )
			obstacleLayerInts.Add ( LayerMask.NameToLayer ( layer ) );
		originalCost = cost;
	}
	
	// COLLISIONS
	void OnCollisionEnter2D ( Collision2D collider )
	{
		if ( obstacleLayerInts.Contains ( collider.gameObject.layer ) )
		{
			cost = 2000f;
		}
	}
	
	void OnCollisionExit2D ( Collision2D collider )
	{
		if ( obstacleLayerInts.Contains ( collider.gameObject.layer ) )
		{
			cost = originalCost;
		}
	}
}
