using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class apple : MonoBehaviour
{
    public GameObject spriteObject;
    public Vector2 direction;
    public GameObject fragmentObject;
    public int fragmentCount;
    public float deflectionMultiplier;
    public float deflectionMaxAngleDeviation;
    public float deflectionMaxSpeedDeviation;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = (Rigidbody2D) GetComponent("Rigidbody2D");
        rb.velocity = direction;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (fragmentObject != null) {
            float normalAngle;
            {
                Vector2 normal = collision.GetContact(0).normal;
                normalAngle = Mathf.Atan2(normal.y, normal.x);
            }
            Collider2D[] colliders = new Collider2D[fragmentCount];
            for (int i = 0; i < fragmentCount; i++) {
                GameObject fragment = Instantiate(fragmentObject, transform);
                fragment.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-180, 180));
                float angle = normalAngle+Random.Range(-deflectionMaxAngleDeviation, deflectionMaxAngleDeviation);
                float vel = rb.velocity.magnitude*(deflectionMultiplier*(1+Random.Range(-deflectionMaxSpeedDeviation, deflectionMaxSpeedDeviation)));
                ((Rigidbody2D) fragment.GetComponent("Rigidbody2D")).velocity = new Vector2(Mathf.Cos(angle)*vel, Mathf.Sin(angle)*vel);
                colliders[i] = (Collider2D) fragment.GetComponent("Collider2D");
                for (int o = 0; o < i; o++) {
                    Physics2D.IgnoreCollision(colliders[i], colliders[o]);
                }
            }
        }
        Destroy(spriteObject);
        Destroy(this);
    }
}
