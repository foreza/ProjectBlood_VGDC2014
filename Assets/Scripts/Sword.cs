using UnityEngine;
using System.Collections;

public enum SwordState
{
    STANDBY,
    SWINGING
}

public class Sword : Weapon
{
    public float swingTime = 5.0f;
    public SwordState state = SwordState.STANDBY;
    public float damage = 50.0f;
    public Transform handlePosition;
    private NoisemakerPlayer noisemaker;
    private Character swordWielder;
    private float swingSpeed = 1000;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // Use this for initialization
    void Start()
    {
        noisemaker = transform.Find("ParticleEffects").GetComponent<NoisemakerPlayer>();
        swordWielder = transform.parent.GetComponent<Character>(); 
        handlePosition = this.transform.parent.FindChild("HandlePosition");
    }

    public override void Attack()
    {
        Swing();
    }

    public void Swing()
    {
        if (this.renderer.enabled == false)
        {
            this.audio.Play();
            originalPosition = this.transform.localPosition;
            originalRotation = this.transform.localRotation;
            StartCoroutine("SwingMotion");
            noisemaker.Noise(9999999);
        }
    }

    public override void Unsheathe(bool swordOut)
    {
        this.renderer.enabled = swordOut;
        this.collider2D.enabled = swordOut;
    }

    IEnumerator SwingMotion()
    {
        state = SwordState.SWINGING;

        this.renderer.enabled = true;
        this.collider2D.enabled = true;

        float currentTime = 0.0f;
 
        while (state == SwordState.SWINGING)
        {
            currentTime += Time.deltaTime;
           
            if (currentTime >= this.swingTime)
            {
                this.renderer.enabled = false;
                this.collider2D.enabled = false;
                state = SwordState.STANDBY;
                //Get the sword back to its original position and rotation once the sword swing is done
                this.transform.localPosition = originalPosition;
                this.transform.localRotation = originalRotation;
            } else {
                //simple sword swing until animations are working
                transform.RotateAround(handlePosition.position, this.transform.forward, Time.deltaTime * swingSpeed);
            }
            yield return null;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (swordWielder.tag == "Player")
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<Enemy>().GetHit(damage);
            } 
            else if (other.gameObject.tag == "EnemyBoss")
            {
                other.gameObject.SetActive(false); //replace with actual damage system
                Application.LoadLevel(4);
            }
        } 
        else if ((swordWielder.tag == "Enemy" || swordWielder.tag == "EnemyBoss") && other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().takeHit(damage);
        }

    }
}
