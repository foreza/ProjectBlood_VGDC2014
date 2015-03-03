using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarMoveTest : MonoBehaviour {

	public Transform start;
	public Transform end;

	private LinkedList<Vector3> path;

	void Start ()
	{
		path = AStarPathing.GetPath ( start, end,  1f );
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ( path != null )
		{
			DrawPath ();
			if ( !this.transform.position.Equals ( path.First.Value ) )
				Vector3.MoveTowards ( this.transform.position, path.First.Value, 5f * Time.deltaTime );
			else if ( path.Count > 0 )
				path.RemoveFirst ();
		}
	}

	private void DrawPath ()
	{
		Vector3 prev = this.transform.position;
		foreach ( Vector3 vec in path )
		{
			Debug.DrawLine ( prev, vec );
			prev = vec;
		}
	}
}
