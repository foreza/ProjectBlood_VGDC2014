using UnityEngine;
using System.Collections;

//Scaling will only scale 1-1, if you increase both dimensions. If you want it longer, you have to edit the texture.
public class UIBar : MonoBehaviour {
	public int sizex;
	public int sizey;
	public int barToDisplayPercent;
	public Texture2D progbarempty;
	public Texture2D progbarfull;
	public int offsetx, offsety;
	void OnGUI(){
		//RGB+Alpha. This affects ALL UI elements. Workaround until UISkin materializes in the assets directory.
		GUI.backgroundColor = new Color (1.0f, 1.0f,1.0f, 0.0f);
		//draw the background
		GUI.BeginGroup (new Rect (offsetx, offsety, sizex, sizey));
			GUI.Box (new Rect (0, 0, sizex, sizey), progbarempty);
			//draw the filling
			GUI.BeginGroup (new Rect (0, 0, sizex * barToDisplayPercent/100, sizey));
				GUI.Box (new Rect (0, 0, sizex - 2, sizey - 2), progbarfull);
			GUI.EndGroup ();
		GUI.EndGroup ();


	}
}
