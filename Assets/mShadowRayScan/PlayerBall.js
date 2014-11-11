//#pragma strict

private var speed:float=0.02;

function Start () {

}

function Update () 
{

	// controls
	if (Input.GetKey ("w"))
	{
		transform.position+=Vector3(0,0,speed);
	}
	if (Input.GetKey ("s"))
	{
		transform.position+=Vector3(0,0,-speed);
	}
	if (Input.GetKey ("a"))
	{
		transform.position+=Vector3(-speed,0,0);
	}
	if (Input.GetKey ("d"))
	{
		transform.position+=Vector3(speed,0,0);
	}

}