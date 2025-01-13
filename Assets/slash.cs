using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class slash : MonoBehaviour
{
    public float speed;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(speed,0,0);
        rb.mass = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SendMessage("DamageHitframes", 12);
    }
}
