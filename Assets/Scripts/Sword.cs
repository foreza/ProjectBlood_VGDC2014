using UnityEngine;
using System.Collections;

public enum SwordState
{
	STANDBY,
	SWINGING
}

public class Sword : MonoBehaviour 
{
	public float swingTime = 5.0f;
	public SwordState state = SwordState.STANDBY;

	public void Swing()
	{
		if(this.renderer.enabled == false)
		{
			StartCoroutine("SwingMotion");
		}
	}

	IEnumerator SwingMotion()
	{
		state = SwordState.SWINGING;

		this.renderer.enabled = true;
		this.collider2D.enabled = true;

		float currentTime = 0.0f;
		while(state == SwordState.SWINGING)
		{
			currentTime = currentTime + Time.deltaTime;
			if(currentTime >= this.swingTime)
			{
				this.renderer.enabled = false;
				this.collider2D.enabled = false;
				state = SwordState.STANDBY;
			}
			yield return null;
		}

	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log ("collided");
		if(other.gameObject.tag == "Enemy")
		{
			Debug.Log ("hit");
			other.gameObject.SetActive(false); //replace with actual damage system
			Debug.Log ("disabled");
		}
	}
}
