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
    public float damage = 50.0f;
	public void Swing()
	{
		if(this.renderer.enabled == false)
		{
			this.audio.Play();
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
			other.gameObject.GetComponent<Enemy>().GetHit(damage);
		}
		if(other.gameObject.tag == "EnemyBoss")
		{
			Debug.Log ("hit boss.");
			other.gameObject.SetActive(false); //replace with actual damage system
			Debug.Log ("disabled, game win");
            Application.LoadLevel(4);
		}

	}
}
