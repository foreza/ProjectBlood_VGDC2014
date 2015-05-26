using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Player player;
	public bool canMove = true;
	public bool canAim = true;

	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	//Use this instead of update for inputs. This function will be called in the InputHandler's Update function.
	public void UpdateController()
	{
		if (Time.timeScale > 0) {
			if (Input.GetButtonDown("Stealth")) {
				player.Stealth();
			}

			if (Input.GetButton ("Blink")) {
				player.Blink();
				player.unStealth();
			}

            if (Input.GetButtonUp("Blink")) {
                player.BlinkParticleEffects(false);
            }

            if(Input.GetButtonDown("Weapon")) {
                player.Attack();
                player.unStealth();
            }
			else if(Input.GetButton ("Demacia")) {
				player.SpinAttack();
				player.unStealth();
			}

			if(canMove) {
				int x = (int)Input.GetAxisRaw("Horizontal");
				int y = (int)Input.GetAxisRaw("Vertical");
				
				Move(x, y);
			}

			Aim();
		}
	}

	void Move(int x, int y) {
		float displace = player.speed * Time.deltaTime;
		float xComp = x * displace;
		float yComp = y * displace;

        // If we are moving diagonally, slow movement along both axes
		if(x != 0 && y != 0) {
			xComp = xComp * Mathf.Sqrt(2)/2;
			yComp = yComp * Mathf.Sqrt(2)/2;
		}

		Vector2 velocity = new Vector2(xComp,yComp);
		GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + velocity);
	}

	void Aim() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector3 lookPos = new Vector3 (mousePos.x-transform.position.x, mousePos.y-transform.position.y, 0);
		transform.up = lookPos;
	}

}
