using UnityEngine;
using System.Collections;

/*
 * Locks a camera to a square aspect ratio.
 * Since the screen is likely non-square, all relative references (including viewport coordinates)
 * will be stretched - which is bad for viewports that were intended to show only a square area (minimaps,  etc.)
 * We compensate by finding what percentage of the screen width is necessary to create a square area, then
 * updating the camera viewport rectangle during each frame.
 */
public class LockSquareAspectRatio : MonoBehaviour
{	
	public float viewportX;
	public float viewportY;
	public bool centerCamera; // If true, center the camera on the point (viewportX, viewportY)
	public Camera targetCamera; // Target camera
	public float cameraVerticalScale; // Desired camera scale between 0 (invisible) and 1 (fills screen vertically)
	

    float currentAspectRatio; // Current aspect ratio
    float prevAspectRatio;
    //internal vars that hold our final viewport size
    float vX;
    float vY;
    float vW;
    float vH;

	// Use this for initialization
	void OnGUI ()
	{
        currentAspectRatio = ((float)Screen.width / (float)Screen.height);
		targetCamera = gameObject.GetComponent<Camera> ();
	}

	// Update is called once per frame
	void Update ()
	{
        if (targetCamera.enabled)
        {
            currentAspectRatio = (((float)Screen.width / (float)Screen.height));
            if (!(prevAspectRatio == currentAspectRatio))
            {
                // aspect ratio compensation
                vW = cameraVerticalScale * (1 / currentAspectRatio);
                vH = cameraVerticalScale;

                if (centerCamera)
                {
                    vX = viewportX - vW / 2;
                    vY = viewportY - vH / 2;
                }
                else
                {
                    vX = viewportX;
                    vY = viewportY;
                }
                targetCamera.rect = new Rect(vX, vY, vW, vH);
            }
        } 
        prevAspectRatio = currentAspectRatio;
	}
}

