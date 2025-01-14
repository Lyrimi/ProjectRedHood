using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWanderer : EntityBase
{
    public GameObject enemy;
    public int damage;

    public SpriteRenderer sr;

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
        sr.flipX = !facingRight;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (grounded && Physics2D.Raycast(new Vector2(transform.position.x+dropDetectOffset.x*(facingRight ? 1 : -1), transform.position.y+dropDetectOffset.y), Vector2.down, dropLimit, dropLayers).collider == null) {
            facingRight = !facingRight;
            sr.flipX = !facingRight;
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
        GameObject other = collision.collider.gameObject;
        if (other == gameObject) {
            other = collision.otherCollider.gameObject;
        }
        if ((wallCheckLayers.value&(1<<other.layer)) != 0) {
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            foreach (ContactPoint2D contact in contacts) {
                if (Mathf.Abs(contact.normal.y) <= wallNormalMaxY) {
                    facingRight = contact.normal.x > 0;
                    sr.flipX = !facingRight;
                }
                break;
            }
        }
    }

    new void OnCollisionStay2D(Collision2D collision) {
        base.OnCollisionStay2D(collision);
        GameObject other = collision.collider.gameObject;
        if (other == gameObject) {
            other = collision.otherCollider.gameObject;
        }
        if (other == enemy) {
            enemy.SendMessage("DamageHitframes", damage);
        }
    }
}
