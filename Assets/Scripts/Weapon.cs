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
    int flipped = 1;

    // Start is called before the first frame update
    private void OnEnable()
    {
        //Subscribes the event functions to the functions in this script
        Input.ShootEvent += inputShoot;
    }
    private void inputShoot()
    {
        GameObject CurrentProjectile = Instantiate(projectile, transform.position, transform.rotation);
        CurrentProjectile.SendMessage("setDirection",new Vector2(ProjectileSpeed*flipped, YSpeed));
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), CurrentProjectile.GetComponent<Collider2D>());
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
            flipped = 1;
        }
        else
        {
            flipped = -1;
        }
        
    }
}
