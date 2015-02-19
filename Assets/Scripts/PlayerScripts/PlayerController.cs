using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Player player;
	public bool canMove = true;
	public bool canAim = true;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find ("Player").GetComponent<Player>();


	}

	//Use this instead of update for inputs. This function will be called in the InputManager's Update function.
	public void UpdateController()
	{
		if (Input.GetButtonDown("Stealth")){
			player.Stealth();
		}

		 if (Input.GetButton ("Blink")) // Will work on this feature more - consider it a fun thing for now.
		{
			player.Blink();
		}
         if (Input.GetButtonUp("Blink"))
         {
             player.BlinkParticleEffects(false);
         }
		 if(Input.GetButtonDown("Weapon"))// attack!
		{
			player.weapon.Attack();
		}

		else if(Input.GetButton ("Demacia"))
		{
			player.Demacia();
		}

		if(canMove)
		{
			int x = (int)Input.GetAxisRaw("Horizontal");
			int y = (int)Input.GetAxisRaw("Vertical");
			
			Move(x, y);
		}

		Aim ();

	}

	void Move(int x, int y)
	{
		float displace = player.speed * Time.deltaTime;
		float xComp = x * displace;
		float yComp = y * displace;

		if(x != 0 && y != 0)
		{
			xComp = xComp * Mathf.Sqrt(2)/2;
			yComp = yComp * Mathf.Sqrt(2)/2;

		}

		Vector2 velocity = new Vector2(xComp,yComp);
		//CharacterController controller = GetComponent<CharacterController> ();
		//controller.Move(velocity);
		//this.gameObject.transform.Translate(velocity,Space.World);
		this.rigidbody2D.MovePosition((Vector2)this.transform.position + velocity);
	}

	void Aim()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector3 lookPos = new Vector3 (mousePos.x-this.transform.position.x, mousePos.y-this.transform.position.y, 0);
		this.transform.up = lookPos;
	}
}
