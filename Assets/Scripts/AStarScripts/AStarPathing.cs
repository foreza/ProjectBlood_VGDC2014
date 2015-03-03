using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
	OPEN = priority queue containing START
	CLOSED = empty set
	while lowest rank in OPEN is not the GOAL:
	  current = remove lowest rank item from OPEN
	  add current to CLOSED
	  for neighbors of current:
	    cost = g(current) + movementcost(current, neighbor)
	    if neighbor in OPEN and cost less than g(neighbor):
	      remove neighbor from OPEN, because new path is better
	    if neighbor in CLOSED and cost less than g(neighbor): **
	      remove neighbor from CLOSED
	    if neighbor not in OPEN and neighbor not in CLOSED:
	      set g(neighbor) to cost
	      add neighbor to OPEN
	      set priority queue rank to g(neighbor) + h(neighbor)
	      set neighbor's parent to current

	reconstruct reverse path from goal to start
	by following parent pointers
*/

public static class AStarPathing
{
	private static AStarNode[] nodes;
	
	public static LinkedList<Vector3> GetPath ( Transform start, Transform end, float averageCost )
	{
		// Initialize if needed
		if ( nodes == null )
			nodes = GameObject.FindObjectsOfType<AStarNode> ();
		//Debug.Log( "Found " + nodes.Length + " A star nodes.");

		bool getSuccess;	// for dictionary queries

		// Set up the known cost function (g-function)
		Dictionary<Transform, float> gFunct = new Dictionary<Transform, float> ();

		// Set up the closed and the unexamined AND unscheduled set of nodes
		HashSet<Transform> others = new HashSet<Transform> ();
		HashSet<Transform> closed = new HashSet<Transform> ();

		// Link node transforms to neighbors
		Dictionary<Transform, AStarNode[]> neighborDict = new Dictionary<Transform, AStarNode[]> ();
		foreach ( AStarNode node in nodes )
		{
			gFunct.Add ( node.transform, node.cost );
			neighborDict.Add ( node.transform, node.neighbors );
			if ( node.transform != start )		// do not add start node to others, it's in the open queue.
				others.Add ( node.transform );
		}

		// Set up the waypoint priority queue
		WaypointPriorityQueue open = new WaypointPriorityQueue ();
		float startCost;
		getSuccess = gFunct.TryGetValue ( start, out startCost );
		if ( !getSuccess )
			BreakWithMessage ( start, "is missing from known cost dictionary!" );
		open.Enqueue ( start, startCost );
		
		// Set up dictionary to keep track of parent/child links
		Dictionary<Transform, Transform> childToParent = new Dictionary<Transform, Transform> ();

		// while lowest priority node is not the goal ...
		//Debug.Log ("Beginning A Star");
		Transform curr;
		do
		{
			// Get next node. Remove from open
			if ( open.Count > 0 )
				curr = open.Dequeue ().Key;
			else
				return null;

			// Do not consider nodes that are in the "others" set. Simulates removal from open queue
			if ( !others.Contains ( curr ) )
			{
				// Add current node to closed
				closed.Add ( curr );
				
				// Try to get the current known cost
				float gcurr;
				getSuccess = gFunct.TryGetValue ( curr, out gcurr );
				if ( !getSuccess )
					BreakWithMessage ( curr, "is missing from known cost dictionary!" );

				// Try to get the current neighbors
				AStarNode[] neighborOut;
				getSuccess = neighborDict.TryGetValue ( curr, out neighborOut );
				if ( !getSuccess )
					BreakWithMessage ( curr, "is missing from neighbor dictionary!" );

				Debug.Log ( "(" + curr.position.x + ", " + curr.position.y + ") known cost: " + gcurr );

				// Scan neighbors and process them
				foreach ( AStarNode neighbor in neighborOut )
				{
					// Get values
					float cost = gcurr + neighbor.cost;
					Transform nt = neighbor.transform;

					// Try to get neighbor's known cost
					float nknowncost;
					getSuccess = gFunct.TryGetValue ( nt, out nknowncost );
					if ( !getSuccess )
						BreakWithMessage ( nt, "is missing from known cost dictionary!" );

					Debug.Log ( "\t(" + nt.position.x + ", " + nt.position.y + ") cost: " + neighbor.cost + " known cost: " + nknowncost );

					// Process the neighbor
					if ( !others.Contains ( nt ) && cost < nknowncost )
					{
						others.Add ( nt );
						Debug.Log ( "\t\tadded to others (removing it from open), new path is better" );
					}
					if ( closed.Contains ( nt ) && cost < nknowncost )
					{
						closed.Remove ( nt );
						//open.Enqueue ( nt, cost );
						Debug.Log ( "\t\removed from closed, added back to open" );
					}
					if ( others.Contains ( nt ) && !closed.Contains ( nt ) )
					{
						gFunct[nt] = cost;
						others.Remove ( nt );
						float dn = Vector3.Distance ( curr.position, nt.position );
						float hn = EstimateCost ( start, end, dn, averageCost );
						float newpriority = cost + hn;
						open.Enqueue ( nt, newpriority );
						if ( !childToParent.ContainsKey ( nt ) )
							childToParent.Add ( nt, curr );
						else
							childToParent[nt] = curr;

						Debug.Log ( "\t\tadded to open with priority " + newpriority );
						Debug.Log ( "\t\tcost changed to " + cost );
						Debug.Log ( "\t\t(" + curr.position.x + ", " + curr.position.y + ") -> (" + nt.position.x + ", " + nt.position.y + ")");
					}
				}
			}
		}
		while ( curr != end );

		// Reconstruct path from goal to start using parents
		LinkedList<Vector3> result = new LinkedList<Vector3> ();
		do
		{
			// Add current node to the path
			result.AddFirst ( curr.position );
			Vector3 prev = curr.position;

			// Try to get the next node in the path
			getSuccess = childToParent.TryGetValue ( curr, out curr );
			if ( !getSuccess )
				BreakWithMessage ( curr, "is missing from parent dictionary!" );
		}
		while ( curr != start );

		Debug.Log ( "Found path of length: " + result.Count );

		return result;
	}

	private static float EstimateCost ( Transform start, Transform end, float neighborDistance, float averageCost )
	{
		return Vector3.Distance ( start.position, end.position ) * averageCost / neighborDistance;
	}

	private static void BreakWithMessage ( Transform pos, string msg )
	{
		Debug.LogError ( "(" + pos.position.x + ", " + pos.position.y + ") " + msg );
		Debug.DebugBreak ();
	}
}

