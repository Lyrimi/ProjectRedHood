using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWanderer : EntityBase
{
    public bool facingRight;
    public float dropLimit;
    public Vector2 dropDetectOffset;
    public LayerMask dropLayers;
    public float wallNormalMaxY;
    public float accel;
    public float maxSpeed;
    public float brakeSpeed;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (grounded && Physics2D.Raycast(new Vector2(transform.position.x+dropDetectOffset.x*(facingRight ? 1 : -1), transform.position.y+dropDetectOffset.y), Vector2.down, dropLimit, dropLayers).collider == null) {
            facingRight = !facingRight;
        }
        float movement; 
        if (Mathf.Sign(rb.velocity.x) != (facingRight ? 1 : -1)) {
            movement = Mathf.Min(Math.Max(accel, brakeSpeed), Mathf.Abs(rb.velocity.x)+accel);
            Debug.Log("a: "+Mathf.Abs(rb.velocity.x));
        } else if (Mathf.Abs(rb.velocity.x) > maxSpeed) {
            movement = -Mathf.Min(Mathf.Abs(rb.velocity.x), brakeSpeed);
            Debug.Log("b: "+Mathf.Abs(rb.velocity.x));
        } else {
            movement = accel;
            Debug.Log("c: "+accel);
        }
        Debug.Log(movement);
        rb.AddForce(new Vector2(facingRight ? movement : -movement, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);
        foreach (ContactPoint2D contact in contacts) {
            if (Mathf.Abs(contact.normal.y) <= wallNormalMaxY) {
                facingRight = contact.normal.x > 0;
                break;
            }
        }
    }
}
