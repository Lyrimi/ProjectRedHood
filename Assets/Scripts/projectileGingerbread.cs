using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class projectileGingerbread : MonoBehaviour
{
    public Vector2 direction;
    public float angularVel;
    public float angularDrag;
    public float deflectionMultiplier;
    public float shrinkTime;
    Rigidbody2D rb;
    Vector3 originalScale;
    Boolean collided = false;
    float collisionTimestamp;

    // Start is called before the first frame update
    void Start()
    {
        rb = (Rigidbody2D) GetComponent("Rigidbody2D");
        rb.velocity = direction;
        rb.angularVelocity = angularVel;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    void Update() {
        if (collided) {
            float time = Time.time-collisionTimestamp;
            if (time >= shrinkTime) {
                Destroy(gameObject);
            } else {
                transform.localScale = originalScale*(1 - time/shrinkTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collided) {
            collided = true;
            Vector2 deflection = collision.GetContact(0).normal;
            deflection *= direction.magnitude*deflectionMultiplier;
            rb.velocity = deflection;
            rb.gravityScale = 1;
            rb.angularDrag = angularDrag;
            collisionTimestamp = Time.time;
        }
    }
}
