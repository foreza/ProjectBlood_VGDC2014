using UnityEngine;
using System.Collections;

public class HealthPackScript : MonoBehaviour {
    public float health = 30;
    protected SpriteRenderer sprite;
	// Use this for initialization
	void Start () {
        sprite = transform.GetComponent<SpriteRenderer>();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("health!");
        if (other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<Player>().takeHit(-health);
            this.audio.Play();
            this.enabled = false;
            this.sprite.enabled = false;
            this.collider2D.enabled = false;
        }
    }
}
