using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.EventSystems.StandaloneInputModule;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    SpriteRenderer spRend;
    [Header("Projectile")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float ProjectileSpeed;
    [SerializeField] private float YSpeed;

    [Header("Input")]
    [SerializeField] private InputManager Input;
    int fliped = 1;

    // Start is called before the first frame update
    private void OnEnable()
    {
        //Subscribes the event functions to the functions in this script
        Input.ShootEvent += inputShoot;
    }
    private void inputShoot()
    {
        GameObject CurentProjectile = Instantiate(projectile, transform.position, transform.rotation);
        CurentProjectile.SendMessage("setDirection",new Vector2(ProjectileSpeed*fliped, YSpeed));
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), CurentProjectile.GetComponent<Collider2D>());
    }

    void Start()
    {
        spRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spRend.flipX == false)
        {
            fliped = 1;
        }
        else
        {
            fliped = -1;
        }
        
    }
}
