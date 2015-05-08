using UnityEngine;
using System.Collections;

public class TreeDamage : MonoBehaviour {

    public float damage = 50.0f;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().takeHit(damage); // We should take damage from the enemy.
        }

    }
}
