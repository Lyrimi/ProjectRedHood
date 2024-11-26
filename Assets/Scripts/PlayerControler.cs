using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerControler : entity_base
{
    SpriteRenderer spRend;
    [Header("Horzontal Movment")]
    [SerializeField] private float MaxSpeed;
    [SerializeField] private float SpeedAcel;
    [SerializeField] private float friction;

    [Header("Jump")]
    [SerializeField] private int MaxCoyoteTime;
    [SerializeField] private int JumpAcelTime;
    [SerializeField] private float jumpforce;

    [Header("Dash")]
    [SerializeField] private float DashVelocity;
    [SerializeField] private int DashFrames;
    [Tooltip("seconds")]
    [SerializeField] private float DashCooldown;

    [Header("Input")]
    [SerializeField] private InputManager Input;

    int coyoteTime;
    int acelTime;
    int dashtime;
    float currentDashCooldown;

    Vector2 moveDir;
    Vector2 dashDir;
    bool isJumping;
    bool isDashing;

    private void OnEnable()
    {
        //Subscribes the event functions to the functions in this script
        Input.MoveEvent += inputMove;
        Input.JumpEvent += inputJump;
        Input.JumpEventCancel += inputJumpCancel;
        Input.DashEvent += inputDash;
        Input.DashEventCancel += inputDashCancel;
        Input.ShootEvent += inputShoot;
    }

    

    // Start is called before the first frame update
    void Start()
    {
        //get components
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<Renderer>();
        spRend = GetComponent<SpriteRenderer>();
    }


    //This Handles Input Events
    private void inputMove(Vector2 direction)
    {
        moveDir = direction;
    }
    private void inputJump() 
    {
        isJumping = true;
    }
    private void inputJumpCancel() 
    {
        isJumping= false;
    }
    private void inputDash() 
    {
        isDashing = true;
    }
    private void inputDashCancel() 
    {
        isDashing= false;
    }
    private void inputShoot() 
    {
        
    }

    //runs every frame
    private void Update()
    {
        //reduces current Dashcooldown by using DeltaTime to take the time in secounds insted of frames
        if (currentDashCooldown > 0)
        {
            currentDashCooldown -= Time.deltaTime;
        }
        if (currentDashCooldown < 0) { currentDashCooldown = 0; }
    }
    //This runs once every physics frame
    void FixedUpdate()
    {
        //Reduces cyote time in frames
        if (grounded)
        {
            coyoteTime = MaxCoyoteTime;
        }
        else if (coyoteTime > 0) 
        {
            coyoteTime -= 1;
        }

        // These call the functions in charge of moving the player
        jumpMove();
        HorizontalMove();
        Dash();
    }

    void jumpMove()
    {
        //gets button and checks coyotetime
        if (isJumping && coyoteTime > 0)
        {
            //sets acel time for the maxmium time the force of the jump can be apllied
            acelTime = JumpAcelTime;
            coyoteTime = 0;
            rb.AddForce(new Vector2(0, jumpforce));
        }
        //While jump button is pressed and aceltime is still active make the y force equal to the jumpforce
        if (isJumping && acelTime > 0)
        {
            rb.AddForce(new Vector2(0, ((jumpforce) - rb.velocity.y)));
            acelTime -= 1;
        }
        //when button is realsed set acel time to 0
        else
        {
            acelTime = 0;
        }
    }
    void HorizontalMove()
    {
        float axis = moveDir.x;
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
    void Dash() 
    {
        if (isDashing && currentDashCooldown <= 0) 
        {
            dashtime = DashFrames;
            dashDir = moveDir;
            currentDashCooldown = DashCooldown;
        }
        if (dashtime > 0) 
        {
            rb.velocity = new Vector2(dashDir.x * DashVelocity, 0);
            dashtime -= 1;
        }
    }
}
