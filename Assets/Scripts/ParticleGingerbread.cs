using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGingerbread : MonoBehaviour
{
    public float shrinkTime;

    Vector2 originalScale;
    float collisionTimestamp;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        collisionTimestamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time-collisionTimestamp;
        if (time >= shrinkTime) {
            Destroy(gameObject);
        } else {
            transform.localScale = originalScale*(1 - time/shrinkTime);
        }
    }
}
