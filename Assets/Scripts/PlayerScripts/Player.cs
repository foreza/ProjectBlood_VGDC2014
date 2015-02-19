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
    public float energyRegenRate = 50.0f;
    public bool energyRegen = true;
    public PlayerState state;
    public SpriteRenderer sprite;
    public Weapon weapon;
    public AudioClip dmgClip;
    public AudioClip stealthClip;
    public Sprite normalSprite;
    public Sprite stealthedSprite;
    private float stealthDegenRate = 10.0f;
	private float spinDegenRate = 0.5f;
	private float blinkDegenCost = 10.0f;

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
                energy += (energyRegenRate * Time.deltaTime * 10);
        }
    }

    void killPlayer()
    {
        this.gameObject.SetActive(false);
        Application.LoadLevel(Application.loadedLevel);
        // StartCoroutine("LoadStartScreen"); 

    }

    public void takeHit(float dmg)
    {
        this.health = this.health - dmg;
		this.audio.clip = this.dmgClip;
		this.audio.Play();
        if (this.health < 0)
        {
            this.health = 0;
        }
    }

	public void Blink()
	{
		if (this.energy > blinkDegenCost) {
						Vector3 target;
						target = transform.position;
						target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						target.z = this.transform.position.z;
						this.transform.position = Vector3.MoveTowards (transform.position, target, speed * Time.deltaTime * 5);
						this.energy -= blinkDegenCost;
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
			this.energy -= spinDegenRate * Time.deltaTime;
			energyRegen = false;
            yield return null;
        }
			
        this.weapon.Unsheathe(false);
		energyRegen = true;
    }

    public void Stealth()
    {
        if (this.state == PlayerState.NORMAL)
        {
            StartCoroutine("StealthRoutine");
        } else if (this.state == PlayerState.STEALTH)
        {
            //make a function for this later
			this.energyRegen = true;
            this.sprite.sprite = normalSprite;
            this.state = PlayerState.NORMAL;
            this.gameObject.audio.Play();
        }
    }
	
    IEnumerator StealthRoutine()
    {
		
        state = PlayerState.STEALTH;
        //		float stealthTime = player.meldTime;
        sprite.sprite = stealthedSprite;
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
            //takeHit(50);
          
            Debug.Log("Health: " + this.health);
        }
    }

    IEnumerator LoadStartScreen()
    { // not working as intended oh well.
	
        yield return new WaitForSeconds(1);


        Application.LoadLevel(0);
    }



}
