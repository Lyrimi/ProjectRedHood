using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class entity_base : MonoBehaviour
{
    internal Rigidbody2D rb;
    internal Renderer render;
    public int MaxHealth = 3;
    public int MaxHitFrames;
    internal bool grounded;
    internal float gravity = 1f;
    internal int health;
    internal int hitFrames;

    // Start is called before the first frame update
    void Start()
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
    private void FixedUpdate()
    {
        if (hitFrames > 0) 
        {
            hitFrames--;
        }
    }

    public void damage(int damage) 
    {
        if (hitFrames <= 0) 
        {
            health -= damage;
        }
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
}
