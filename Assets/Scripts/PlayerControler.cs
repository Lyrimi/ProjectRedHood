using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControler : entity_base
{
    SpriteRenderer spRend;
    public int MaxCoyoteTime;
    public int JumpAcelTime;
    public float jumpforce;
    public float MaxSpeed;
    public float SpeedAcel;
    public float friction;
    int CoyoteTime;
    int AcelTime;
    // Start is called before the first frame update
    void Start()
    {
        //get components
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<Renderer>();
        spRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        if (grounded)
        {
            CoyoteTime = MaxCoyoteTime;
        }
        else if (CoyoteTime > 0) 
        {
            CoyoteTime -= 1;
        }

        //Jump logic
        jumpMove();

        //horizontal movment
        HorizontalMove();
    }

    void jumpMove()
    {
        //gets button and checks coyotetime
        if (Input.GetButton("Jump") && CoyoteTime > 0)
        {
            //sets acel time for the maxmium time the force of the jump can be apllied
            AcelTime = JumpAcelTime;
            CoyoteTime = 0;
            rb.AddForce(new Vector2(0, jumpforce));
        }
        //While jump button is pressed and aceltime is still active make the y force equal to the jumpforce
        if (Input.GetButton("Jump") && AcelTime > 0)
        {
            rb.AddForce(new Vector2(0, ((jumpforce) - rb.velocity.y)));
            AcelTime -= 1;
        }
        //when button is realsed set acel time to 0
        else
        {
            AcelTime = 0;
        }
    }
    void HorizontalMove()
    {
        float axis = Input.GetAxisRaw("Horizontal");
        float x = axis * SpeedAcel;
        //adds speed if the speed goes over speed maxspeed set speed to max speed
        float veloshouldbe = rb.velocity.x + x;
        rb.AddForce(new Vector2(x, 0));
        if (Mathf.Abs(rb.velocity.x) >= MaxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -1, 1) * MaxSpeed, rb.velocity.y);
        }
        
        //Fiction logic
        if (Mathf.Abs(rb.velocity.x) > 0 & math.abs(axis) == 0)
        {
            rb.AddForce(new Vector2(Mathf.Clamp(rb.velocity.x, -1, 1) * -(friction), 0));
        }


        //Animation State change
        if (x != 0)
        {
            //anim.SetBool("Walking", true);
        }
        else
        {
            //anim.SetBool("Walking", false);
        }

        //sprite Flip
        if (axis > 0)
        {
            spRend.flipX = false;
        }
        if (axis < 0)
        {
            spRend.flipX = true;
        }
    }
}
