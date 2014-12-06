using UnityEngine;
using System.Collections;
public enum PlayerState
{
	NORMAL,
	STEALTH
}

public class PlayerController : MonoBehaviour {

	private Player player;
	private Sword sword;
	public PlayerState state;
	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find ("Player").GetComponent<Player>();
		sword = GameObject.Find ("Sword").GetComponent<Sword> ();

		state = PlayerState.NORMAL;
	}
	
	// Update is called once per frame
	void Update () 
	{
		int x = (int)Input.GetAxisRaw("Horizontal");
		int y = (int)Input.GetAxisRaw("Vertical");
		
		move(x, y);
		aim ();
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			stealth();
		}

		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			sword.Swing ();
		}


	}
	void stealth()
	{
		if (state == PlayerState.NORMAL) {

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
		CharacterController controller = GetComponent<CharacterController> ();
		controller.Move(velocity);
		//this.gameObject.transform.Translate(velocity,Space.World);
	}

	void aim()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector3 lookPos = new Vector3 (mousePos.x-this.transform.position.x, mousePos.y-this.transform.position.y, 0);
		this.transform.right = lookPos;
	}
}
