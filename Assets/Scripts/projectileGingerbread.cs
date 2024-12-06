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

    Rigidbody2D rb;
    bool collided;
    Vector2 contactNormal;

    // Start is called before the first frame update
    void Start()
    {
        rb = (Rigidbody2D) GetComponent("Rigidbody2D");
        rb.velocity = direction;
        rb.angularVelocity = angularVel;
        handleIsAlly();
        transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-180, 180));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    void Update() {
        if (collided) {
            float angle = 2*Mathf.Atan2(contactNormal.y, contactNormal.x)-Mathf.Atan2(direction.y, direction.x)+Mathf.PI;
            Vector2 deflection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            deflection *= direction.magnitude*deflectionMultiplier;

            GameObject particle = Instantiate(hitParticle, transform.position, transform.rotation);
            particle.GetComponent<Rigidbody2D>().velocity = deflection;

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collided) {
            collided = true;
            contactNormal = collision.GetContact(0).normal;
            GetComponent<Collider2D>().enabled = false;
            rb.simulated = false;
        }
    }
}
