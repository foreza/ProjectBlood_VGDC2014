using UnityEngine;
using System.Collections;

public class RandomTeleport : AbstractSkill {
    public Transform teleportOrigin;
    public float maxRange;
    private float timer = 0;
    public float cooldown = 10;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    if (timer > 0) {
			timer += Time.deltaTime;
			if (timer >= cooldown) {
				timer = 0;
			}
		}
	}

    public override void Use()
    {
        if (timer > 0) //the ability is still on cooldown
        {
            return;
        }
        Vector2 teleportDirection = Random.insideUnitCircle * maxRange;
        Vector2 newTestPosition = (Vector2)teleportOrigin.position  + teleportDirection;
        while (Physics2D.OverlapCircle(newTestPosition, (GetComponent<CircleCollider2D>().radius))) //while position is invalid, make a new position
        {
            teleportDirection = Random.insideUnitCircle * maxRange;
            newTestPosition = (Vector2)(transform.position) + teleportDirection;
        }
        transform.position = newTestPosition;
        timer += Time.deltaTime;
    }
}
