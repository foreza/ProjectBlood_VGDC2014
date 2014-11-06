using UnityEngine;
using System.Collections;

/*
 * Locks the camera to a square aspect ratio, compensating for the screen's aspect ratio.
 * The tCamScale var determines how much of the screen the camera's viewport will take up. 
 */
public class LockSquareAspectRatio : MonoBehaviour
{
		public Camera tCam; // Target camera
		public float tCamScale; // Desired camera scale between 0 (invisible) and 1 (fills screen vertically)
		float cAspRatio; // Current aspect ratio

		// Use this for initialization
		void OnGUI ()
		{
				cAspRatio = ((float)Screen.width / (float)Screen.height);
				tCam = gameObject.GetComponent<Camera>();
		}
	
		// Update is called once per frame
		void Update ()
		{
				cAspRatio = (((float)Screen.width / (float)Screen.height));
				// set the camera viewpoint to a new rectangle of the appropriate size, limiting the horizontal size
				tCam.rect = new Rect (tCam.rect.x, tCam.rect.y, tCamScale * (1 / cAspRatio), tCamScale);
		}
}

