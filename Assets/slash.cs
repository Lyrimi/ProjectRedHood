using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class slash : MonoBehaviour
{
    IEnumerator Expire(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        Destroy(this);
    }
    public float speed;
    Rigidbody2D rb;
    public float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Expire(lifetime));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("DamageHitframes", 12);
        }
    }
    
}
