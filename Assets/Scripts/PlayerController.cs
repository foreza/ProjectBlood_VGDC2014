using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Player player;
	private Sword sword;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find ("Player").GetComponent<Player>();
		sword = GameObject.Find ("Sword").GetComponent<Sword> ();


	}
	
	// Update is called once per frame
	void Update ()
	{
		aim ();
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			stealth();
		}
		
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			sword.Swing ();
		}
	}

	void FixedUpdate () 
	{
		int x = (int)Input.GetAxisRaw("Horizontal");
		int y = (int)Input.GetAxisRaw("Vertical");
		
		move(x, y);



	}
	void stealth()
	{
		Debug.Log ("shift pressed");
		if (player.state == PlayerState.NORMAL) {
			StartCoroutine("StealthRoutine");
		}
	}

	IEnumerator StealthRoutine()
	{
		Debug.Log ("StealthRoutine");
		player.state = PlayerState.STEALTH;
		float stealthTime = 5.0f;
		player.sprite.enabled = false;

		Debug.Log ("Sprite toggled");

		float currentTime = 0.0f;

		while(player.state == PlayerState.STEALTH)
		{
			Debug.Log (currentTime);
			Debug.Log (stealthTime);
			currentTime = currentTime + Time.deltaTime;
			if(currentTime >= stealthTime)
			{

				Debug.Log ("true");
				Debug.Log (currentTime);
				Debug.Log (stealthTime);
				player.sprite.enabled = true;
				player.state = PlayerState.NORMAL;
			}
			yield return null;
		}
		

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
		//CharacterController controller = GetComponent<CharacterController> ();
		//controller.Move(velocity);
		this.gameObject.transform.Translate(velocity,Space.World);
	}

	void aim()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector3 lookPos = new Vector3 (mousePos.x-this.transform.position.x, mousePos.y-this.transform.position.y, 0);
		this.transform.up = lookPos;
	}
}
