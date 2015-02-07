using UnityEngine;
using System.Collections;

public enum PlayerState
{
    NORMAL,
    STEALTH,
}


public class Player : Character
{
    public float energy = 50;
	public float healthMax = 100;
    public float energyMax = 50;
    public float energyRegenRate = 1.0f;
    public bool energyRegen = true;
    public PlayerState state;
    public SpriteRenderer sprite;
    public Weapon weapon;
    public AudioClip dmgClip;
    public AudioClip stealthClip;

    private float stealthDegenRate = 10.0f;

    void Start()
    {
        sprite = transform.FindChild("PlayerPlaceholder").GetComponent<SpriteRenderer>();
        weapon = this.transform.FindChild("Sword").GetComponent<Sword>();
        state = PlayerState.NORMAL;
		health = healthMax;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (this.health <= 0)
        {
            killPlayer();
        }
        if (energyRegen)
        {
            if (energy <= energyMax)
                energy += (energyRegenRate * Time.deltaTime);
        }
    }

    void killPlayer()
    {
        this.gameObject.SetActive(false);
        Application.LoadLevel(3);
        // StartCoroutine("LoadStartScreen"); 

    }

    public void takeHit(float dmg)
    {
        this.health = this.health - dmg;
        if (this.health < 0)
        {
            this.health = 0;
        }
    }

    public void Demacia()
    {
        StartCoroutine("DemaciaRoutine");
    }

    IEnumerator DemaciaRoutine()
    {
        while (Input.GetButton("Demacia"))
        {
            this.weapon.Unsheathe(true);
            this.gameObject.transform.Rotate(Vector3.forward, 30.0f, Space.Self);
            yield return null;
        }
			
        this.weapon.Unsheathe(false);
    }

    public void Stealth()
    {
        if (this.state == PlayerState.NORMAL)
        {
            StartCoroutine("StealthRoutine");
        } else if (this.state == PlayerState.STEALTH)
        {
            //make a function for this later
			this.energyRegen = false;
            this.sprite.enabled = true;
            this.state = PlayerState.NORMAL;
            this.gameObject.audio.Play();
        }
    }
	
    IEnumerator StealthRoutine()
    {
		
        state = PlayerState.STEALTH;
        //		float stealthTime = player.meldTime;
        sprite.enabled = false;
        energyRegen = false;
        audio.clip = this.stealthClip;
        this.audio.Play();
		
        //		float currentTime = 0.0f;
		
        while (this.state == PlayerState.STEALTH)
        {
            this.energy -= stealthDegenRate * Time.deltaTime;
            if (this.energy <= 0)
            {
                this.energy = 0;
                this.energyRegen = true;
                this.sprite.enabled = true;
                this.state = PlayerState.NORMAL;
                this.audio.Play();
            }
            yield return null;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            takeHit(10);
            this.audio.clip = this.dmgClip;
            //this.audio.Play();
            Debug.Log("Health: " + this.health);
        }
    }

    IEnumerator LoadStartScreen()
    { // not working as intended oh well.
	
        yield return new WaitForSeconds(1);


        Application.LoadLevel(0);
    }


}
