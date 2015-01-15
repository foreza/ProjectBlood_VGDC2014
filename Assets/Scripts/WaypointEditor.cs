using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(WaypointGraph))]
public class WaypointEditor : Editor
{
	
	public WaypointGraph waypointGraph;
	private string fromStr = "";
	private string toStr = "";
	private string waypointStr = "";
	

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		waypointGraph = (WaypointGraph)target;
		
		if(waypointGraph == null || waypointGraph.waypoints == null || waypointGraph.edges == null) 
		{
			waypointGraph.InitializeGraph();
		}
		else if(waypointGraph.isEmpty())
		{	
			waypointGraph.PopulateGraph();
		}
			
		waypointStr = EditorGUILayout.TextField("Waypoint: ", waypointStr);
		
		if(GUILayout.Button("Add Waypoint"))
		{
			waypointGraph.CreateWaypoint(waypointStr);
			
			waypointStr = "";
			Repaint();
		}
			
		fromStr = EditorGUILayout.TextField ("From: ", fromStr);
		toStr = EditorGUILayout.TextField (  "To: ", toStr);
		
		if(GUILayout.Button("Add Edge"))
		{
		
			waypointGraph.CreateEdge(fromStr, toStr);
			
			fromStr = "";
			toStr = "";
			Repaint();
		}
		
		SceneView.RepaintAll();
	}
	
	void OnSceneGUI()
	{
		if(waypointGraph != null && waypointGraph.graphIsOutDated())
		{
			waypointGraph.PopulateGraph();
		}
		
		if(waypointGraph != null)
		{
			waypointGraph.DrawGraph();
		}
		
		
	}
}
