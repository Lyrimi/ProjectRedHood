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
    internal bool grounded;
    internal float gravity = 1f;
    internal int health;
    internal int hitFrames;

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
    //physics update
    internal void FixedUpdate()
    {
        if (hitFrames > 0)
        {
            hitFrames = hitFrames - 1;
        }
    }

    public void Damage(int damage) 
    {
        health -= damage;
        if (health <= 0) {
            gameObject.SendMessage("Death", damage);
        }
    }

    public void DamageHitframes(int damage) 
    {
        if (hitFrames <= 0) 
        {
            //this is dumb
            gameObject.SendMessage("Damage", damage);
            hitFrames = MaxHitFrames;
            StartCoroutine(hitcolor(MaxHitFrames));
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
    private void OnCollisionStay2D(Collision2D collision)
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
    IEnumerator hitcolor (int imuntyframes)
    {
        render.material.SetColor("_Color", Color.red);
        yield return StartCoroutine(WaitForFixedFrames(imuntyframes));
        render.material.SetColor("_Color", Color.white);
    }

    IEnumerator WaitForFixedFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
        {
            yield return new WaitForFixedUpdate();
        }
    }
}
