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
    public LayerMask wallCheckLayers;
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
        } else if (Mathf.Abs(rb.velocity.x) > maxSpeed) {
            movement = -Mathf.Min(Mathf.Abs(rb.velocity.x), brakeSpeed);
        } else {
            movement = accel;
        }
        rb.AddForce(new Vector2(facingRight ? movement : -movement, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);
        foreach (ContactPoint2D contact in contacts) {
            if (Mathf.Abs(contact.normal.y) <= wallNormalMaxY) {
                GameObject other = contact.collider.gameObject;
                if (other == gameObject) {
                    other = contact.otherCollider.gameObject;
                }
                if ((wallCheckLayers.value&(1<<other.layer)) != 0) {
                    facingRight = contact.normal.x > 0;
                }
                break;
            }
        }
    }
}
