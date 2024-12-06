using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileApple : ProjectileBase
{
    public GameObject spriteObject;
    public GameObject fragmentObject;
    public int fragmentCount;
    public float deflectionMultiplier;
    public float deflectionMultiplierRange;
    public float minScatterSpeed;
    public float maxScatterSpeed;

    Rigidbody2D rb;
    bool collided;
    Vector2 contactNormal;

    // Start is called before the first frame update
    void Start()
    {
        rb = (Rigidbody2D) GetComponent("Rigidbody2D");
        rb.velocity = direction;
        handleIsAlly();
    }

    void Update() {
        if (collided) {
            if (fragmentObject != null) {
                /*Vector2 surfaceNormal = collision.GetContact(0).normal;
                float deflectionAngle = 2*Mathf.Atan2(surfaceNormal.y, surfaceNormal.x)-Mathf.Atan2(rb.velocity.y, rb.velocity.x)+Mathf.PI;
                Vector2 deflection = new Vector2(Mathf.Cos(deflectionAngle), Mathf.Sin(deflectionAngle))*rb.velocity.magnitude;*/
                Vector2 deflection = contactNormal*rb.velocity.magnitude;
                for (int i = 0; i < fragmentCount; i++) {
                    GameObject fragment = Instantiate(fragmentObject, transform.position, Quaternion.identity);
                    Vector2 vel = deflection*deflectionMultiplier*(1+Random.Range(-deflectionMultiplierRange, deflectionMultiplierRange));
                    float angle = Random.Range(-180, 180);
                    float scatterSpeed = Random.Range(minScatterSpeed, maxScatterSpeed);
                    fragment.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle)*scatterSpeed, Mathf.Sin(angle)*scatterSpeed)+vel;
                }
            }
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!collided) {
            collided = true;
            contactNormal = collision.GetContact(0).normal;
            GetComponent<Collider2D>().enabled = false;
            rb.simulated = false;
        }
    }
}
