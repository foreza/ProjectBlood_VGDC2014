//David Gonzalez
//ProjectBlood


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointGraph : MonoBehaviour 
{
	
	private Dictionary<Transform,List<Transform>> graph;
	public Transform waypoints;
	public Transform edges;
	private int edgeCount;

//	WaypointGraph()
//	{
//		InitializeGraph();
//	}
	
	void Start()
	{
		InitializeGraph();
		PopulateGraph();
	}
	
	public void InitializeGraph()
	{
		graph = new Dictionary<Transform,List<Transform>>();
		
		if(waypoints == null)
		{
			waypoints = this.transform.FindChild("Waypoints");
		}
		
		if(edges == null)
		{
			edges = this.transform.FindChild("Edges");
		}
		
		edgeCount = 0;
	}
	
	public void PopulateGraph()
	{
		if(graph == null)
		{
			graph = new Dictionary<Transform,List<Transform>>();
		}
		
		Clear();
		
		for(int i = 0; i < waypoints.childCount; i++)
		{
			Transform waypoint = waypoints.transform.GetChild(i);
			AddWaypoint(waypoint);
		}
		
		TrimDirtyEdges();
		
		for(int i = 0; i < edges.childCount; i++)
		{
			Transform edge = edges.transform.GetChild(i);
			string[] pairStr = edge.name.Split('_');
			string fromStr = pairStr[0];
			string toStr = pairStr[1];
			Transform fromObj = waypoints.transform.FindChild(fromStr);
			Transform toObj = waypoints.transform.FindChild(toStr);
			
			AddEdge(fromObj, toObj);
			
		}
		
		Debug.Log("Instantiated");
		Debug.Log("waypoints: " + WaypointCount() + " " + waypoints.childCount);
		Debug.Log("edges: " + EdgeCount() + " " + edges.childCount);
	}
	
	//Implements A* search to find the shortest path from startNode to goalNode
	public LinkedList<Transform> ShortestPath(Transform startNode, Transform goalNode)
	{
		Dictionary<Transform, Transform> cameFrom = new Dictionary<Transform,Transform>();
		Dictionary<Transform, float> gScore = new Dictionary<Transform, float>();
		Dictionary<Transform, float> fScore = new Dictionary<Transform, float>();
		Dictionary<Transform, float> evaluatedNodes = new Dictionary<Transform,float>();
		WaypointPriorityQueue fringeQueue = new WaypointPriorityQueue();
		
		
		gScore[startNode] = 0.0f;
		fScore[startNode] = gScore[startNode] + heuristicCost(startNode, goalNode);
		fringeQueue.Enqueue(startNode, fScore[startNode]);
		
		while(fringeQueue.Count != 0)
		{
			Debug.Log("LoopCount");
			Transform currentNode = fringeQueue.Dequeue().Key;
			
			if(currentNode == goalNode)
			{
				Transform current = currentNode;
				List<Transform> totalPath = new List<Transform>();
				totalPath.Add(current);
				Debug.Log(current.name);
				while(cameFrom.ContainsKey(current))
				{
					current = cameFrom[current];
					totalPath.Add(current);
					Debug.Log(current.name);
				}
			
				return null; // return shortest path
			}
			
			evaluatedNodes.Add(currentNode, fScore[currentNode]);
			
			foreach(Transform neighbor in GetNeighborList(currentNode))
			{
				if(!evaluatedNodes.ContainsKey(neighbor))
				{
					float travelCost = gScore[currentNode] + (currentNode.position - neighbor.position).magnitude;
					bool inFringe = fringeQueue.ContainsWaypoint(neighbor);
					
					if(!inFringe || travelCost < gScore[neighbor])
					{
						cameFrom[neighbor] = currentNode;
						gScore[neighbor] = travelCost;
						fScore[neighbor] = gScore[neighbor] + heuristicCost(neighbor, goalNode);
						
						fringeQueue.Enqueue(neighbor, fScore[neighbor]);
						
//						if(!inFringe)
//						{
//							fringeQueue.Enqueue(neighbor, fScore[neighbor]);
//						}
					}
				}
			}
		}
		
		return null;
	}
	
	public void DrawGraph()
	{
		
		if(waypoints != null && edges != null)
		{
			for(int i = 0; i < waypoints.childCount; i++)
			{
				Transform waypoint = waypoints.GetChild(i);
				List<Transform> neighborList = GetNeighborList(waypoint);
				
				if(neighborList != null)
				{				
					foreach(Transform neighbor in neighborList)
					{
						Vector3 from = waypoint.transform.position;
						Vector3 to = neighbor.transform.position;
						
						UnityEditor.Handles.DrawLine(from, to);
					}
				}
			}
		}
	}
	
	public void CreateWaypoint(string waypointStr)
	{
		Transform waypoint = waypoints.FindChild(waypointStr);
		
		if(waypoint == null)
		{
			GameObject waypointObj = new GameObject();
			waypointObj.name = waypointStr;
			waypointObj.transform.parent = waypoints;
			AddWaypoint(waypointObj.transform);
		}
	}
	
	public void CreateEdge(string fromStr, string toStr)
	{
		GameObject edge = new GameObject();
		edge.name = fromStr + "_" + toStr;
		edge.transform.parent = edges;
		AddEdge(fromStr,toStr);
	}

	public bool isEmpty()
	{
		if(graph == null || graph.Count == 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public int WaypointCount()
	{
		int count = 0;
		
		if(graph != null)
		{
			count = graph.Count;
		}
		
		return count;
	}
	
	public int EdgeCount()
	{
		return edgeCount;
	}

	public void Clear()
	{
		graph.Clear();
		edgeCount = 0;
	}
	
	public bool graphIsOutDated()
	{
		bool isOld = false;
		
		if(waypoints != null && edges != null)
		{
			if(   WaypointCount() != waypoints.childCount
			   || EdgeCount() != edges.childCount )
			{
				
				isOld = true;
			}
		}
		
		return isOld;
	}
	
	private bool AddWaypoint(Transform waypoint)
	{
		try
		{
			graph.Add(waypoint, new List<Transform>());
		}
		catch(System.ArgumentException)
		{
			return false;
		}
		
		return true;
	}
	
	private bool AddEdge(string fromStr, string toStr)
	{
		
		Transform fromObj =waypoints.FindChild(fromStr);
		Transform toObj = waypoints.FindChild(toStr);
		
		return AddEdge(fromObj, toObj);
	}
	
	//Adds a new edge if the two specified waypoints exists and the edge does not already exist.
	//returns true if succesful, false if unsuccesful
	private bool AddEdge(Transform from, Transform to)
	{
		bool isValid = false;
		
		if(    graph.ContainsKey(from) && graph.ContainsKey (to)
		   && !graph[from].Contains(to) && !graph[to].Contains(from) )
		{
			graph[from].Add(to);
			graph[to].Add(from);
			isValid = true;
			edgeCount++;
		}
		
		
		return isValid;
	}
	
	private void TrimDirtyEdges()
	{
		Queue<Transform> dirtyEdgeQueue = new Queue<Transform>();
		
		for(int i = 0; i < edges.childCount; i++)
		{
			Transform edge = edges.transform.GetChild(i);
			string[] pairStr = edge.name.Split('_');
			
			if(pairStr.GetLength(0) != 2)
			{
				dirtyEdgeQueue.Enqueue(edge);
			}
			else
			{
				string fromStr = pairStr[0];
				string toStr = pairStr[1];
				Transform fromObj = waypoints.FindChild(fromStr);
				Transform toObj = waypoints.FindChild(toStr);
				
				if(fromObj == null || toObj == null)
				{
					dirtyEdgeQueue.Enqueue(edge);
				}
			}
		}
		
		while(dirtyEdgeQueue.Count > 0)
		{
			GameObject toDestroy = dirtyEdgeQueue.Dequeue().gameObject;
			DestroyImmediate(toDestroy);
		}
	}
	
	
	
	private List<Transform> GetNeighborList(Transform waypoint)
	{
		List<Transform> neighborList = null;
		
		if(graph.ContainsKey(waypoint))
		{
			neighborList = graph[waypoint];
		}
		
		return neighborList;
	}
	
	private float heuristicCost(Transform fromNode, Transform toNode)
	{
		return (fromNode.position - toNode.position).magnitude;
	}
}
