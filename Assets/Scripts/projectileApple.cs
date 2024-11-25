using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class apple : MonoBehaviour
{
    public GameObject spriteObject;
    public Vector2 direction;
    public GameObject fragmentObject;
    public int fragmentCount;
    public float deflectionMultiplier;
    public float deflectionMultiplierRange;
    public float minScatterSpeed;
    public float maxScatterSpeed;
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
            Vector2 deflection = collision.GetContact(0).normal*rb.velocity.magnitude;
            Collider2D[] colliders = new Collider2D[fragmentCount];
            for (int i = 0; i < fragmentCount; i++) {
                GameObject fragment = Instantiate(fragmentObject, transform.position, Quaternion.Euler(0, 0, Random.Range(-180, 180)));
                Vector2 vel = deflection*deflectionMultiplier*(1+Random.Range(-deflectionMultiplierRange, deflectionMultiplierRange));
                float angle = Random.Range(-180, 180);
                float scatterSpeed = Random.Range(minScatterSpeed, maxScatterSpeed);
                Rigidbody2D fragmentRb = ((Rigidbody2D) fragment.GetComponent("Rigidbody2D"));
                fragmentRb.velocity = new Vector2(Mathf.Cos(angle)*scatterSpeed, Mathf.Sin(angle)*scatterSpeed)+vel;
                colliders[i] = (Collider2D) fragment.GetComponent("Collider2D");
                for (int o = 0; o < i; o++) {
                    Physics2D.IgnoreCollision(colliders[i], colliders[o]);
                }
            }
        }
        Destroy(spriteObject);
        Destroy(gameObject);
    }
}
