using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Player player;
	private Sword sword;
	private bool whirl; // for whirling purposes.
	private float stealthDegenRate = 10.0f;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find ("Player").GetComponent<Player>();
		sword = GameObject.Find ("Sword").GetComponent<Sword> ();

	}
	
	// Update is called once per frame
	void Update ()
	{

		if(Input.GetKeyDown(KeyCode.Tab)) {
			whirl = true; // DEMACIA.
		}

		if (Input.GetKeyUp (KeyCode.Tab)) {
						whirl = false;
				}

		if (whirl) {

			sword.Swing (); // sword swing!
			//Vector3 lookPos = new Vector3 (50 - this.transform.position.x, 50 -this.transform.position.y, 0);
			// this.transform.up = lookPos;

			this.gameObject.transform.Rotate(Vector3.forward, 30.0f, Space.Self);

				}

		if (!whirl) {
						aim ();
				}

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
		else if (player.state == PlayerState.STEALTH)
		{
			//make a function for this later
			player.energyRegen = true;
			player.sprite.enabled = true;
			player.state = PlayerState.NORMAL;
			this.audio.Play ();
		}
	}

	IEnumerator StealthRoutine()
	{

		player.state = PlayerState.STEALTH;
//		float stealthTime = player.meldTime;
		player.sprite.enabled = false;
		player.energyRegen = false;
		player.audio.clip = player.stealthClip;
		this.audio.Play ();

//		float currentTime = 0.0f;

		while(player.state == PlayerState.STEALTH)
		{
//			Debug.Log (currentTime);
//			Debug.Log (stealthTime);
//			currentTime = currentTime + Time.deltaTime;
//			if(currentTime >= stealthTime)
			player.energy -= stealthDegenRate*Time.deltaTime;
			if (player.energy <= 0)
			{
				player.energy = 0;
				player.energyRegen = true;
				player.sprite.enabled = true;
				player.state = PlayerState.NORMAL;
				this.audio.Play ();
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
