using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempPlayerController : MonoBehaviour
{
    ParticleSystem PartiSystem;
    SpriteRenderer spRend;
    Animator anim;
    Rigidbody2D rb;
    Renderer player_render;
    public int Speed;
    public int jumpforce;
    public int MaxCoyoteTime;
    int CoyoteTime;
    bool grounded;
    public int MaxHealth = 3;
    int health;
    int DeathTime = 50;
    int Respawn = 0;
    int hit_frames = 0;
    Vector3 startpos;
    public int JumpAccelTime;
    int AccelTime;
    public float gravity = 1;
    int gravScale;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spRend = GetComponent<SpriteRenderer>();
        startpos = this.transform.position;
        health = MaxHealth;
        player_render = GetComponent<Renderer>();

        setGravity(gravity);
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    //logic loop
    private void FixedUpdate()
    {
        HorizontalMove();
        jumpMove();
        gravityManager();
        if (!grounded && CoyoteTime > 0)
        {
            CoyoteTime -= 1;
            Debug.Log(CoyoteTime);
        }
        if (hit_frames > 0)
        {
            hit_frames -= 1;
        }
        else
        {
            player_render.material.SetColor("_Color", Color.white);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hit_frames <= 0)
        {
            health -= 1;
            Debug.Log(health);
            if (health <= 0)
            {
                transform.position = startpos;
                health = MaxHealth;
                Respawn = DeathTime;
                Debug.Log("You died");
            }
            player_render.material.SetColor("_Color", Color.red);
            hit_frames = 15;
           

        }
        if (collision.gameObject.CompareTag("Pickup"))
        {
            Debug.Log("pickup");
            Destroy(collision.gameObject);
            String type = Variables.Object(collision.gameObject).Get("Type").ToString();
            Debug.Log(type);
            if (type == "Health")
            {
                health += 1;
                if (health > MaxHealth)
                {
                    health = MaxHealth;
                }
                Debug.Log("Heal up");
                Debug.Log(health);
                
            }
        }
        if (collision.gameObject.CompareTag("Win"))
        {
            Debug.Log("you won!!");
            String Scene = Variables.Object(collision.gameObject).Get("Scene").ToString();
            SceneManager.LoadScene(Scene);
        }
    }

    //Movments
    void HorizontalMove()
    {
        float x = Input.GetAxis("Horizontal") * Speed;
        rb.AddForce(new Vector2(x - rb.velocity.x, 0));
       
        
        if (x > 0)
        {
            spRend.flipX = false;
        }
        if (x < 0)
        {
            spRend.flipX = true;
        }
    }

    void jumpMove()
    {
        if (Input.GetButton("Jump") && CoyoteTime > 0)
        {
            AccelTime = JumpAccelTime;
            CoyoteTime = 0;
            rb.AddForce(new Vector2(0, jumpforce * gravScale));
        }
        if (Input.GetButton("Jump") && AccelTime > 0)
        {
            rb.AddForce(new Vector2(0, ((jumpforce * gravScale) - rb.velocity.y)));
            AccelTime -= 1;
        }
        else
        {
            AccelTime = 0;
        }
    }
    void gravityManager()
    {
        rb.gravityScale = gravity;
        if (gravity > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (gravity < 0)
        {
            transform.eulerAngles = new Vector3(180, 0, 0);
        }
    }
    
    static int gravityscale(float value)
    {
        value = Math.Clamp(value, -1, 1);
        if (value > 0)
        {
            value = 1;
        }
        else if(value < 0)
        {
            value = -1;
        }
        return (int)value;
    }

    public void setGravity(float grav)
    {
        gravity = grav;
        rb.gravityScale = gravity;
        gravScale = gravityscale(gravity);
    }
}
