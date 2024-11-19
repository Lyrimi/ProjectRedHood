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
    public int jumpforce;
    public int MaxSpeed;
    public int SpeedAcel;
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
        
        float x = Input.GetAxis("Horizontal") * SpeedAcel;
        //gets difrence between current speed and max speed creates a number from 0-1 that the speed(x) is multiplied with
        rb.AddForce(new Vector2(x - rb.velocity.x, 0));

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
        if (x > 0)
        {
            spRend.flipX = false;
        }
        if (x < 0)
        {
            spRend.flipX = true;
        }
    }
}
