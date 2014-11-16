using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Player player;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void FixedUpdate()
	{
		int x = (int)Input.GetAxisRaw("Horizontal");
		int y = (int)Input.GetAxisRaw("Vertical");
		
		move(x, y);
	}

	void move(int x, int y)
	{
		float displace = player.speed * Time.deltaTime;
		float xComp = x * displace;
		float yComp = y * displace;

		if(x != 0 && y != 0)
		{
			xComp = xComp * Mathf.Sqrt(2)/2;
			yComp = yComp * Mathf.Sqrt(2)/2;
		}

		Vector3 velocity = new Vector3(xComp,yComp,0);
		this.gameObject.transform.Translate(velocity);

	}
}
