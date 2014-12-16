using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour 
{

	public abstract void Attack();
	public abstract void Unsheathe(bool weaponOut);
	
}
