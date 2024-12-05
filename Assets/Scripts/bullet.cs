using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }

    void setDirection(Vector2 direction) {
        this.direction = direction;
    }
}
