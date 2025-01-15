using System;
using System.Collections;
using System.Collections.Generic;
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
    public int damage;

    Rigidbody2D rb;
    bool collided;
    Vector2 contactNormal;
    Vector2 contactRelVel;

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
                Vector2 incoming = -contactRelVel.normalized;
                // ## WARNING: Maths ##
                //normal = the normal of the surface
                //incoming = the vector the object is hitting the normal with
                //normal = nx + ny*i
                //incoming = cx + cy*i
                //
                //normal*normal/incoming*-1
                //(nx + ny*i)*(nx + ny*i)/(cx + cy*i)*-1
                //-(nx^2 + 2*nx*ny*i + -ny^2)/(cx + cy*i)
                //-(nx^2 + 2*nx*ny*i + -ny^2)*(cx + -cy*i)/((cx + cy*i)*(cx + -cy*i))
                //-(nx^2*cx + 2*nx*ny*cx*i + -ny^2*cx + -nx^2*cy*i + 2*nx*ny*cy + ny^2*cy*i)/(cx^2 + cy^2)
                //-(nx^2*cx + -ny^2*cx + 2*nx*ny*cy + (2*nx*ny*cx + -nx^2*cy + ny^2*cy)*i)
                //-nx^2*cx + ny^2*cx + -2*nx*ny*cy + (-2*nx*ny*cx + nx^2*cy + -ny^2*cy)*i
                //(-nx^2 + ny^2)*cx + -2*nx*ny*cy + ((nx^2 + -ny^2)*cy + -2*nx*ny*cx)*i
                float a = -contactNormal.x*contactNormal.x+contactNormal.y*contactNormal.y;
                float b = -2*contactNormal.x*contactNormal.y;
                Vector2 deflection = new Vector2(a*incoming.x+b*incoming.y, -a*incoming.y+b*incoming.x)*contactRelVel.magnitude;
                for (int i = 0; i < fragmentCount; i++) {
                    GameObject fragment = Instantiate(fragmentObject, transform.position, Quaternion.identity);
                    Vector2 vel = deflection*deflectionMultiplier*(1+Random.Range(-deflectionMultiplierRange, deflectionMultiplierRange));
                    float angle = Random.Range(-180, 180);
                    float scatterSpeed = Random.Range(minScatterSpeed, maxScatterSpeed);
                    fragment.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle) * scatterSpeed, Mathf.Sin(angle) * scatterSpeed) + vel;
                    
                    Physics2D.IgnoreCollision(fragment.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                }
            }
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
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
