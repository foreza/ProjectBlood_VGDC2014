using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour 
{

	public abstract void Attack(); // Override this with your own attack
	public abstract void Unsheathe(bool weaponOut); // Override with your own unsheathe method.
	public abstract void OnTriggerEnter2D(Collider2D other); // Override this method.
	
}
