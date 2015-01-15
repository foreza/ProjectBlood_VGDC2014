using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointPriorityQueue 
{
	LinkedList<KeyValuePair<Transform,float>> waypointList;
	
	public WaypointPriorityQueue()
	{
		waypointList = new LinkedList<KeyValuePair<Transform, float>>();
	}
	
	public void Enqueue(Transform waypoint, float fScore)
	{
		KeyValuePair<Transform,float> item = new KeyValuePair<Transform, float>(waypoint, fScore);
		
		if(waypointList.Count == 0)
		{
			waypointList.AddFirst(item);
		}
		else 
		{
			LinkedListNode<KeyValuePair<Transform,float>> pairNode = waypointList.First;
			bool found = false;
			
			while(pairNode != null && !found)
			{
				if(item.Value <= pairNode.Value.Value)
				{
					waypointList.AddBefore(pairNode, item);
					found = true;
				}
				else if(pairNode.Next == null)
				{
					waypointList.AddAfter(pairNode, item);
				}
				
				pairNode = pairNode.Next;
			}
		}
	}
	
	public KeyValuePair<Transform,float> Dequeue()
	{
		KeyValuePair<Transform,float> toReturn = waypointList.First.Value;
		
		waypointList.RemoveFirst();
		
		return toReturn;
	}
	
	public bool ContainsWaypoint(Transform waypoint)
	{
		LinkedListNode<KeyValuePair<Transform,float>> pairNode = waypointList.First;
		bool found = false;
		
		while(pairNode != null && !found)
		{
			if(pairNode.Value.Key == waypoint)
			{
				found = true;
			}
			
			pairNode = pairNode.Next;
		}
		
		return found;
	}
	
	public int Count
	{
		get{ return waypointList.Count;}
	}
	
}
