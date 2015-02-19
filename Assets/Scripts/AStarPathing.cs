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
		
		// Set up the known cost function (g-function)
		Dictionary<Transform, float> gFunct = new Dictionary<Transform, float> ();
		// Link node transforms to neighbors
		Dictionary<Transform, AStarNode[]> neighborDict = new Dictionary<Transform, AStarNode[]> ();
		foreach ( AStarNode node in nodes )
		{
			gFunct [node.transform] = node.cost;
			neighborDict [node.transform] = node.neighbors;
		}
		
		// Set up the waypoint priority queue
		WaypointPriorityQueue open = new WaypointPriorityQueue ();
		open.Enqueue ( start, gFunct [start] );
		
		// Set up the closed and not-open set of nodes
		HashSet<Transform> notOpen = new HashSet<Transform> ();
		HashSet<Transform> closed = new HashSet<Transform> ();
		
		// Set up dictionary to keep track of parent/child links
		Dictionary<Transform, Transform> childToParent = new Dictionary<Transform, Transform> ();
		
		// while lowest priority node is not the goal ...
		Transform curr = start;
		while ( curr != end )
		{
			if ( !notOpen.Contains ( curr ) )
			{
				closed.Add ( curr );
				float gcurr = gFunct [curr];
				
				foreach ( AStarNode neighbor in neighborDict[curr] )
				{
					float cost = gcurr + neighbor.cost;
					Transform nt = neighbor.transform;
					if ( !notOpen.Contains ( nt ) && cost < gFunct[nt] )
						notOpen.Add ( nt );
					if ( closed.Contains ( nt ) && cost < gFunct[nt] )
						closed.Remove ( nt );
					if ( notOpen.Contains ( nt ) && !closed.Contains ( nt ) )
					{
						gFunct[nt] = cost;
						notOpen.Remove ( nt );
						float dn = Vector3.Distance ( curr.position, nt.position );
						float hn = EstimateCost ( start, end, dn, averageCost );
						open.Enqueue ( nt, gFunct[nt] + hn );
						childToParent[nt] = curr;
					}
				}
			}
			
			// iterate the while loop
			curr = open.Dequeue ().Key;
		}
		
		// reconstruct path from goal to start using parents
		LinkedList<Vector3> result = new LinkedList<Vector3> ();
		while ( childToParent[curr] != start )
		{
			result.AddFirst ( curr.position );
			curr = childToParent[curr];
		}
		
		return result;
	}
	
	public static float EstimateCost ( Transform start, Transform end, float neighborDistance, float averageCost )
	{
		return Vector3.Distance ( start.position, end.position ) * averageCost / neighborDistance;
	}
}

