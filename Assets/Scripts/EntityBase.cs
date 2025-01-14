using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public abstract class EntityBase : MonoBehaviour
{
    internal Rigidbody2D rb;
    internal Renderer render;
    public int MaxHealth = 36;
    public int MaxHitFrames = 10;
    int hitFrames = 0;
    int hitFlash = 0;
    internal bool grounded;
    internal float gravity = 1f;
    internal int health;
    internal bool hashitFrames = false;

    // Start is called before the first frame update
    internal void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<Renderer>();
        health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void FixedUpdate() {
		if (hitFlash > 0) {
            hitFlash--;
            if (hitFlash == 0) {
                if (hitFrames == 0) {
                    render.material.SetColor("_Color", Color.white);
                } else {
                    render.material.SetColor("_Color", new Color(1, .33333f, .33333f));
                }
            }
        }
        if (hitFrames > 0) {
            hitFrames--;
            if (hitFrames == 0) {
                render.material.SetColor("_Color", Color.white);
            }
        }
	}


	public void Damage(int damage) 
    {
        gameObject.SendMessage("TakeDamage", damage);
        hitFlash = 8;
        render.material.SetColor("_Color", Color.red);
    }

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            gameObject.SendMessage("Death", damage);
        }
    }

    public void DamageHitframes(int damage) 
    {
        if (hitFrames == 0) 
        {
            //this is dumb
            gameObject.SendMessage("TakeDamage", damage);
            hitFrames = MaxHitFrames;
            if (hitFlash == 0) {
                render.material.SetColor("_Color", new Color(1, .33333f, .33333f));
            }
        }
    }

    public void Death() {
        Destroy(gameObject);
    }

    //Ground detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("NotGround")) {
            if (collision.contacts[0].normal.y > 0.8f && gravity > 0)
            {
                grounded = true;
            }
            else if (collision.contacts[0].normal.y < -0.8f && gravity < 0)
            {
                grounded = true;
            }
        }
    }
    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("NotGround")) {
            if (collision.contacts[0].normal.y > 0.8f && gravity > 0)
            {
                grounded = true;
            
            }
            else if (collision.contacts[0].normal.y < -0.8f && gravity < 0)
            {
                grounded = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }
}
