using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEditor.UI;
using UnityEngine.UI;

public class PlayerController : EntityBase
{
    SpriteRenderer spRend;
    [Header("Horzontal Movment")]
    [SerializeField] private float MaxSpeed;
    [SerializeField] private float SpeedAccel;
    [SerializeField] private float friction;
    [SerializeField] private float airFriction;

    [Header("Jump")]
    [SerializeField] private int MaxCoyoteTime;
    [SerializeField] private int JumpAccelTime;
    [SerializeField] private float jumpForce;

    [Header("Dash")]
    [SerializeField] private float DashVelocity;
    [SerializeField] private int DashFrames;
    [Tooltip("seconds")]
    [SerializeField] private float DashCooldown;

    [Header("Input")]
    [SerializeField] private InputManager Input;

    [Header("Heart system")]
    [SerializeField] private Animator DeathAnim;
    public Image[] hearts;
    public Sprite normalHeart;
    public Sprite damagedheart;
    [Header("Other settings")]
    [Tooltip("secounds")] 
    public float hitStoptime;
    public HitStop hitstop;
    

    int coyoteTime;
    int accelTime;
    int dashTime;
    float currentDashCooldown;
    int displayHearts;
    
    //Visible in the editor; janky.
    public int throwDelay;

    Vector2 moveDir;
    Vector2 dashDir;
    bool isJumping;
    bool isDashing;
    Animator anim;
    public GameManager gameManager;
    bool isStandingInSign = false;
    bool isStandingInHouse = false;
    GameObject CurrentSign;
    BoxCollider2D box;

    private void OnEnable()
    {
        //Subscribes the event functions to the functions in this script
        Input.MoveEvent += inputMove;
        Input.JumpEvent += inputJump;
        Input.JumpEventCancel += inputJumpCancel;
        Input.DashEvent += inputDash;
        Input.DashEventCancel += inputDashCancel;
    }

    

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //get components
        spRend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();

        displayHearts = hearts.Length;
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
        if (isStandingInSign)
        {
            CurrentSign.SendMessage("GetContent");
            return;
        }
        if (isStandingInHouse)
        {
            gameManager.nextScene("BossFight");
            return;
        }
        isDashing = true;
    }
    private void inputDashCancel() 
    {
        isDashing= false;
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
        //Reduces coyote time in frames
        if (grounded && rb.velocity.y <= 0)
        {
            coyoteTime = MaxCoyoteTime;
        }
        else if (coyoteTime > 0) 
        {
            coyoteTime -= 1;
        }
        
        if (throwDelay > 0) {
            throwDelay--;
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
            //sets accel time for the maximum time the force of the jump can be applied
            accelTime = JumpAccelTime;
            coyoteTime = 0;
            rb.AddForce(new Vector2(0, jumpForce));
        }
        //While jump button is pressed and acceltime is still active make the y force equal to the jumpForce
        if (isJumping && accelTime > 0)
        {
            rb.AddForce(new Vector2(0, ((jumpForce) - rb.velocity.y)), ForceMode2D.Impulse);
            accelTime -= 1;
        }
        //when button is released set accel time to 0
        else
        {
            accelTime = 0;
        }
    }
    void HorizontalMove()
    {
        float axis = moveDir.x;
        float x = axis * SpeedAccel;
        //adds speed if the speed goes over speed maxspeed set speed to max speed
        //(unused) float veloshouldbe = rb.velocity.x + x;
        rb.AddForce(new Vector2(x, 0));
        if (Mathf.Abs(rb.velocity.x) >= MaxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -1, 1) * MaxSpeed, rb.velocity.y);
        }
        
        //Friction logic
        if (Mathf.Abs(rb.velocity.x) > 0 & axis == 0)
        {
            rb.AddForce(new Vector2(Mathf.Clamp(rb.velocity.x, -1, 1) * -(grounded ? friction : airFriction), 0));
        }


        //Animation State change
        anim.SetFloat("xVelocty", math.abs(rb.velocity.x));

        //sprite Flip
        if (axis > 0)
        {
            spRend.flipX = false;
        } else if (axis < 0)
        {
            spRend.flipX = true;
        }
    }
    void Dash() 
    {
        if (isDashing && currentDashCooldown <= 0) 
        {
            dashTime = DashFrames;
            dashDir = new Vector2(spRend.flipX ? -1 : 1, 0);
            currentDashCooldown = DashCooldown;
        }
        if (dashTime > 0) 
        {
            rb.velocity = new Vector2(dashDir.x * DashVelocity, 0);
            dashTime -= 1;
        }
    }
    public new void Damage(int damage)
    {
        base.Damage(damage);
        int currHearts = displayHearts;
        displayHearts = (int) Mathf.Ceil((float) health/MaxHealth*hearts.Length);
        if (displayHearts < currHearts)
        {
            for (int i = currHearts; i > displayHearts && i > 0; i--) {
                hearts[i-1].sprite = damagedheart;
            }
        }
        if (health > 0)
        {
            hitstop.Stop(hitStoptime);
        }
    }
    public new void Death()
    {
        StartCoroutine(deathEvent());
    }

    IEnumerator deathEvent()
    {
        Time.timeScale = 0;
        DeathAnim.SetTrigger("Dead");
        yield return new WaitForSecondsRealtime(3f);
        Debug.Log("restarted");
        gameManager.nextScene("Start");
        Time.timeScale = 1;
    }

    void SetIsSignPresent(bool IsSign)
    {
        isStandingInSign = IsSign;
    }

    void SetCurrentSign(GameObject sign)
    {
        CurrentSign = sign;
    }
    void SetIsHousePresent(bool isHouse)
    {
        isStandingInHouse = isHouse;
    }

}
