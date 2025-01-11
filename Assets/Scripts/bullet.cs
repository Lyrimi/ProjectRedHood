using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : ProjectileBase
{
    public int damage;

    public float lifetime;
    
    float spawnTime;

    bool collided;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction;
        handleIsAlly();
        spawnTime = Time.time;
    }

    void FixedUpdate()
    {
        if (!collided && Time.fixedTime-spawnTime >= lifetime && lifetime != 0) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collided) {
            collided = true;
            GameObject victim = collision.collider.gameObject;
            if (victim == gameObject) {
                victim = collision.otherCollider.gameObject;
            }
            victim.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}
