using UnityEngine;
using System.Collections;

public enum SwordState
{
	STANDBY,
	SWINGING
}

public class Sword : Weapon
{
	public float swingTime = 5.0f;
	public SwordState state = SwordState.STANDBY;
    public float damage = 50.0f;

	public override void Attack()
	{
		Swing ();
	}

	public void Swing()
	{
		if(this.renderer.enabled == false)
		{
			this.audio.Play();
			StartCoroutine("SwingMotion");
		}
	}

	public override void Unsheathe(bool swordOut)
	{
		this.renderer.enabled = swordOut;
		this.collider2D.enabled = swordOut;
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
		if(other.gameObject.tag == "Enemy")
		{
			other.gameObject.GetComponent<Enemy>().GetHit(damage);
		}
		if(other.gameObject.tag == "EnemyBoss")
		{
			other.gameObject.SetActive(false); //replace with actual damage system
            Application.LoadLevel(4);
		}

	}
}
