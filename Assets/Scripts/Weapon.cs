using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.EventSystems.StandaloneInputModule;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Weapon : MonoBehaviour
{
    public PlayerController player;

    SpriteRenderer spRend;
    [Header("Projectile")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float ProjectileSpeed;
    [SerializeField] private float YSpeed;
    public int throwCooldown;
    public bool key2;

    [Header("Input")]
    [SerializeField] private InputManager Input;

    // Start is called before the first frame update
    private void OnEnable()
    {
        //Subscribes the event functions to the functions in this script
        if (key2) {
            Input.Shoot2Event += inputShoot;
        } else {
            Input.ShootEvent += inputShoot;
        }
    }
    private void inputShoot() {
        if (player.throwDelay == 0) {
            player.throwDelay = throwCooldown;
            GameObject projectile = Instantiate(this.projectile, transform.position, transform.rotation);
            projectile.SendMessage("setDirection", new Vector2(ProjectileSpeed*(spRend.flipX?-1:1), YSpeed));
            projectile.SendMessage("setIsAlly", true);
        }
    }

    void Start()
    {
        spRend = GetComponent<SpriteRenderer>();
    }
}
