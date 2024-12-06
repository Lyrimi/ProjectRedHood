using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileGingerbread : ProjectileBase
{
    public float angularVel;
    public float angularDrag;
    public float deflectionMultiplier;
    public float shrinkTime;

    public int postHitLayer;
    public LayerMask postHitCollisionInclude;
    public LayerMask postHitCollisionExclude;
    public LayerMask postHitForceSend;
    public LayerMask postHitForceReceive;
    public LayerMask postHitCollisionCaptures;
    public LayerMask postHitCollisionCallbacks;

    Rigidbody2D rb;
    Vector2 originalScale;
    Boolean collided = false;
    float collisionTimestamp;

    // Start is called before the first frame update
    void Start()
    {
        rb = (Rigidbody2D) GetComponent("Rigidbody2D");
        rb.velocity = direction;
        rb.angularVelocity = angularVel;
        handleIsAlly();
        originalScale = transform.localScale;
        transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-180, 180));
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
            Vector2 surfaceNormal = collision.GetContact(0).normal;
            float angle = 2*Mathf.Atan2(surfaceNormal.y, surfaceNormal.x)-Mathf.Atan2(direction.y, direction.x)+Mathf.PI;
            Vector2 deflection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            deflection *= direction.magnitude*deflectionMultiplier;
            rb.velocity = deflection;
            rb.gravityScale = 1;
            rb.angularDrag = angularDrag;
            collisionTimestamp = Time.time;

            Collider2D col = GetComponent<Collider2D>();
            col.includeLayers = postHitCollisionInclude;
            col.excludeLayers = postHitCollisionExclude;
            col.forceSendLayers = postHitForceSend;
            col.forceReceiveLayers = postHitForceReceive;
            col.contactCaptureLayers = postHitCollisionCaptures;
            col.callbackLayers = postHitCollisionCallbacks;
            gameObject.layer = postHitLayer;
        }
    }
}
