using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.EventSystems.StandaloneInputModule;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Weapon : MonoBehaviour
{
    SpriteRenderer spRend;
    [Header("Projectile")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float ProjectileSpeed;
    [SerializeField] private float YSpeed;
    public int windupFrames;
    public int totalFrames;

    [Header("Input")]
    [SerializeField] private InputManager Input;
    bool flipped;
    int delay = 0;

    // Start is called before the first frame update
    private void OnEnable()
    {
        //Subscribes the event functions to the functions in this script
        Input.ShootEvent += inputShoot;
    }
    private void inputShoot() {
        if (delay == 0) {
            delay = totalFrames;
            flipped = spRend.flipX;
            if (windupFrames == 0) {
                fireProjectile();
            }
        }
    }

    void Start()
    {
        spRend = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() {
        if (delay > 0) {
            delay--;
            if (delay == totalFrames-windupFrames && windupFrames != 0) {
                fireProjectile();
            }
        }
    }

    void fireProjectile() {
        GameObject projectile = Instantiate(this.projectile, transform.position, transform.rotation);
        projectile.SendMessage("setDirection", new Vector2(ProjectileSpeed*(flipped?-1:1), YSpeed));
        projectile.SendMessage("setIsAlly", true);
    }
}
