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
    private Weapon weapon;
    public AudioClip dmgClip;
    public AudioClip stealthClip;
    public Sprite normalSprite;
    public Sprite stealthedSprite;
    public float stealthDegenRate = 10.0f;
	public float spinDegenRate = 0.5f;
	public float blinkDegenCost = 10.0f;
    private ParticleSystem trailParticles;
    private ParticleSystem blinkParticles;
    private GameObject[] enemyDetectionColliders;
    void Start()
    {
        sprite = transform.FindChild("PlayerPlaceholder").GetComponent<SpriteRenderer>();
        trailParticles = transform.FindChild("PlayerPlaceholder").GetComponent<ParticleSystem>();
        blinkParticles = transform.FindChild("BlinkParticleEffect").GetComponent<ParticleSystem>();
        weapon = this.transform.FindChild("Sword").GetComponent<Sword>();
        enemyDetectionColliders = GameObject.FindGameObjectsWithTag(Tags.LOSCollider);
        setState(PlayerState.NORMAL);
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

    void setState(PlayerState newstate)
    {
        if(state == newstate)
        {
            return;
        }
        if(newstate == PlayerState.STEALTH)
        {
            foreach(GameObject LoSCollider in enemyDetectionColliders)
            {
                LoSCollider.gameObject.SetActive(false);
            }
        }
        else if(newstate == PlayerState.NORMAL)
        {
            foreach(GameObject LoSCollider in enemyDetectionColliders)
            {
                LoSCollider.gameObject.SetActive(true);
            }
        }
        state = newstate;
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
		this.GetComponent<AudioSource>().clip = this.dmgClip;
		this.GetComponent<AudioSource>().Play();
        if (this.health < 0)
        {
            this.health = 0;
        }
    }

	public void Blink()
	{
        if (this.energy > blinkDegenCost)
        {
            BlinkParticleEffects(true);
            Vector3 target;
            target = transform.position;
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = this.transform.position.z;
            this.transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime * 5);
            this.energy -= blinkDegenCost;
        }
        else
        {
            BlinkParticleEffects(false);
        }
	}

    public void BlinkParticleEffects(bool enabled)
    {
        if (trailParticles.enableEmission == true && enabled == true) //edge trigger for first activation
        {
            GetComponent<AudioSource>().clip = this.stealthClip;
            this.GetComponent<AudioSource>().Play();
            blinkParticles.Play();
        }
        trailParticles.enableEmission = !enabled;

    }

    public void Attack()
    {
        weapon.Attack();
        this.energyRegen = true;
        this.sprite.sprite = normalSprite;
        setState(PlayerState.NORMAL);
        this.GetComponent<AudioSource>().Play();
    }

    public void SpinAttack()
    {
        StartCoroutine("DemaciaRoutine");
        this.sprite.sprite = normalSprite;
        setState(PlayerState.NORMAL);
        this.GetComponent<AudioSource>().Play();
    }

    IEnumerator DemaciaRoutine()
    {
        while (Input.GetButton("Demacia"))
        {
            this.weapon.Unsheathe(true);
            this.gameObject.transform.Rotate(Vector3.forward, 30.0f, Space.Self);
			this.energy -= spinDegenRate * Time.deltaTime;
			energyRegen = false;
			if (this.energy <= 0)
			{
				this.energy = 0;
				this.energyRegen = true;
				setState(PlayerState.NORMAL);
				break;
			}
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
            setState(PlayerState.NORMAL);
            this.gameObject.GetComponent<AudioSource>().Play();
        }
    }

	public void unStealth() // called only by other skills that should break you out of stealth.
	{
		this.energyRegen = true; // not sure if this will cause anything;
		this.sprite.sprite = normalSprite;
        setState(PlayerState.NORMAL);
		this.gameObject.GetComponent<AudioSource>().Play();
	}
	

    IEnumerator StealthRoutine()
    {

        setState(PlayerState.STEALTH);
        //		float stealthTime = player.meldTime;
        sprite.sprite = stealthedSprite;
        energyRegen = false;
        GetComponent<AudioSource>().clip = this.stealthClip;
        this.GetComponent<AudioSource>().Play();
		
        //		float currentTime = 0.0f;
		
        while (this.state == PlayerState.STEALTH)
        {
            this.energy -= stealthDegenRate * Time.deltaTime;
            if (this.energy <= 0)
            {
                this.energy = 0;
                this.energyRegen = true;
                this.sprite.sprite = normalSprite;
                setState(PlayerState.NORMAL);
                this.GetComponent<AudioSource>().Play();
            }
            yield return null;
        }
    }

    IEnumerator LoadStartScreen()
    { // not working as intended oh well.
	
        yield return new WaitForSeconds(1);


        Application.LoadLevel(0);
    }



}
