using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : entity_base
{
    public int MaxCoyoteTime;
    public int JumpAcelTime;
    public int jumpforce;
    int CoyoteTime;
    int AcelTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<Renderer>();
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

        Debug.Log(CoyoteTime);
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
}
