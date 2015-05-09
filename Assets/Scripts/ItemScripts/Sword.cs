using UnityEngine;
using System.Collections;

public enum SwordState
{
    STANDBY,
    SWINGING
}

public class Sword : Weapon
{
    public float swingTime = 5.0f; 					// time it takes to swing.
    public SwordState state = SwordState.STANDBY;	// standby = sheathed?
    public float damage = 50.0f;					// damage of sword. 
    private Transform handlePosition;				// position of handle.
    public float noiseRadius = 125.0f;
    private NoisemakerPlayer noisemaker;
    private Transform swordWielder;
    public float swingSpeed = 1000;
    public float critMultiplier = 3;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // Use this for initialization
    void Start()
    {
        noisemaker = transform.Find("ParticleEffects").GetComponent<NoisemakerPlayer>();
        swordWielder = transform.parent;
        handlePosition = this.transform.parent.FindChild("HandlePosition");
    }

    public override void Attack()
    {
				Swing ();			 							// No need to call another method; just define it here.
    }		

    public void Swing()
    {
        if (this.GetComponent<Renderer>().enabled == false) 					// if the weapon is not yet out.
        {
            this.GetComponent<AudioSource>().Play();									// Play the sound
            originalPosition = this.transform.localPosition;	// Transform/move the sword
            originalRotation = this.transform.localRotation;	// Rotate it
            StartCoroutine("SwingMotion");						// Call the co-routine "SwingMotion"
            
        }
    }

    public override void Unsheathe(bool swordOut)
    {
        this.GetComponent<Renderer>().enabled = swordOut;
        this.GetComponent<Collider2D>().enabled = swordOut;
    }

    IEnumerator SwingMotion()
    {
        state = SwordState.SWINGING;

        this.GetComponent<Renderer>().enabled = true;							// Set the weapon to be visible.
        this.GetComponent<Collider2D>().enabled = true;							// Set collider to true to allow us to damage things.

        float currentTime = 0.0f;
 
        while (state == SwordState.SWINGING)
        {
            currentTime += Time.deltaTime;
           
            if (currentTime >= this.swingTime)
            {
                this.GetComponent<Renderer>().enabled = false;
                this.GetComponent<Collider2D>().enabled = false;
                state = SwordState.STANDBY;
                //Get the sword back to its original position and rotation once the sword swing is done
                this.transform.localPosition = originalPosition;
                this.transform.localRotation = originalRotation;
            } else {
                //simple sword swing until animations are working
                this.transform.RotateAround(handlePosition.position, this.transform.forward, Time.deltaTime * swingSpeed);
            }
            yield return null;
        }
    }

	public override void OnTriggerEnter2D(Collider2D other)
    {

        if (swordWielder.tag == "Player") 
        {
            if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
            {
				if(other.gameObject.GetComponent<Enemy>().state == EnemyState.PATROL || other.gameObject.GetComponent<Enemy>().state == EnemyState.STATIONARY)
                {
                    other.gameObject.GetComponent<Enemy>().GetHit(critMultiplier*damage);
                    Debug.Log("crit!");
                }
                else
                    other.gameObject.GetComponent<Enemy>().GetHit(damage);  // Deal damage to the enemy.

                noisemaker.Noise(noiseRadius);						// Cause nose for the radius.
            } 
            else if (other.gameObject.tag == "EnemyBoss")
            {
                other.gameObject.SetActive(false); 					   //replace with actual damage system
                Application.LoadLevel(4); // finish.
            }
        } 
        else if ((swordWielder.tag == "Enemy" || swordWielder.tag == "Boss") && other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().takeHit(damage); // We should take damage from the enemy.
        }

    }
}
