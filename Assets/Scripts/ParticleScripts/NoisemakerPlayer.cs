using UnityEngine;
using System.Collections;

// general note: for the settings of the particle system, 750 base speed on .1 second run, then 0.4 dampening gives you 100 distance.

public class NoisemakerPlayer : MonoBehaviour {
	private string[] soundLayers = {"Enemy"};
	private string[] lightLayers = {"Enemy","LightWalls"};
    private ParticleSystem particleEffect;
	// Use this for initialization
	void Start () {
	    particleEffect =  transform.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Noise(float radius)
	{
		RaycastHit2D[] hits  = Physics2D.CircleCastAll(transform.position, radius, new Vector2(0,0), 0, LayerMask.GetMask(soundLayers));
		foreach (RaycastHit2D hit in hits)
		{
			if (hit.collider.tag == "Enemy" || hit.collider.tag == "Boss") {
				if(hasLoS (transform, hit.transform, (int)radius, LayerMask.GetMask(lightLayers)))
				{
                    hit.transform.GetComponent<Enemy>().OnPlayerSighted();
				}
			}
		}
        particleEffect.startSpeed = 7.50f * radius;
        particleEffect.Play();
	}
	public bool hasLoS(Transform one, Transform two, int distance, LayerMask mask)
	{
		Vector3 rayDirection = two.position - one.position;
		RaycastHit2D hit = Physics2D.Raycast (one.position, rayDirection, distance, mask);
		
		if(hit.transform.Equals (two)) //there is nothing else that it hit between them
		{
			return true;
		}
		return false;
	}
	public bool hasLoS(Transform one, Transform two, LayerMask mask)
	{
		Vector3 rayDirection = two.position - one.position;
		RaycastHit2D hit = Physics2D.Raycast (one.position, rayDirection, int.MaxValue, mask);

		if(hit.transform.Equals (two)) //there is nothing else that it hit between them
		{
				return true;
		}
		return false;
	}
	public bool hasLoS(Transform one, Transform two) //don't use this if Transform one has it's own collider; it'll collide with itself
	{
		Vector3 rayDirection = two.position - one.position;
		RaycastHit2D hit = Physics2D.Raycast (one.position, rayDirection);

		if(hit.transform.Equals (two)) //there is nothing else that it hit between them
		{
			return true;
		}
		return false;
	}
}

