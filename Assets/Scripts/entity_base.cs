using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class entity_base : MonoBehaviour
{
    Rigidbody2D rb;
    Renderer render;
    public int MaxHealth = 3;
    public int MaxCoyoteTime;
    public int MaxHitFrames;
    internal bool grounded;
    internal int CoyoteTime;
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
        if (!grounded && CoyoteTime < 0)
        {
            CoyoteTime--;
        }
    }

    public void damage(int damage) 
    {
        if (hitFrames <= 0) 
        {
            health = health - damage;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.8f && gravity > 0)
        {
            grounded = true;
            CoyoteTime = MaxCoyoteTime;
        }
        else if (collision.contacts[0].normal.y < -0.8f && gravity < 0)
        {
            grounded = true;
            CoyoteTime = MaxCoyoteTime;
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.8f && gravity > 0)
        {
            grounded = true;
            CoyoteTime = MaxCoyoteTime;
        }
        else if (collision.contacts[0].normal.y < -0.8f && gravity < 0)
        {
            grounded = true;
            CoyoteTime = MaxCoyoteTime;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }
}
