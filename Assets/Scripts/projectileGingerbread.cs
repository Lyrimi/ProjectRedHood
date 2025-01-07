using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileGingerbread : ProjectileBase
{
    public float angularVel;
    public float deflectionMultiplier;

    public GameObject hitParticle;
    public int damage;

    public float lifetime;

    Rigidbody2D rb;
    bool collided;
    Vector2 contactNormal;
    Vector2 contactRelVel;
    float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = (Rigidbody2D) GetComponent("Rigidbody2D");
        rb.velocity = direction;
        rb.angularVelocity = angularVel;
        handleIsAlly();
        transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-180, 180));
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!collided && Time.fixedTime-spawnTime >= lifetime && lifetime != 0) {
            Destroy(gameObject);
        }
    }

    void Update() {
        if (collided) {
            if (hitParticle != null) {
                Vector2 incoming = -contactRelVel.normalized;
                //See maths section in ProjectileApple for more information.
                float a = -contactNormal.x*contactNormal.x+contactNormal.y*contactNormal.y;
                float b = -2*contactNormal.x*contactNormal.y;
                Vector2 deflection = new Vector2(a*incoming.x+b*incoming.y, -a*incoming.y+b*incoming.x)*contactRelVel.magnitude*deflectionMultiplier;

                GameObject particle = Instantiate(hitParticle, transform.position, transform.rotation);
                particle.GetComponent<Rigidbody2D>().velocity = deflection;
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collided) {
            collided = true;
            contactNormal = collision.GetContact(0).normal;
            contactRelVel = collision.relativeVelocity;
            GetComponent<Collider2D>().enabled = false;
            rb.simulated = false;
            GameObject victim = collision.collider.gameObject;
            if (victim == gameObject) {
                victim = collision.otherCollider.gameObject;
            }
            victim.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }
}
