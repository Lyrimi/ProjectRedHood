using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileGingerbread : MonoBehaviour
{
    public Vector2 direction;
    public float angularVel;
    public float angularDrag;
    public float deflectionMultiplier;
    Rigidbody2D rb;
    Boolean collided = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = (Rigidbody2D) GetComponent("Rigidbody2D");
        rb.velocity = direction;
        rb.angularVelocity = angularVel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collided) {
            collided = true;
            Vector2 deflection = collision.GetContact(0).normal;
            deflection *= direction.magnitude*deflectionMultiplier;
            rb.velocity = deflection;
            rb.gravityScale = 1;
            rb.angularDrag = angularDrag;
        }
    }
}
