using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileAppleFragment : MonoBehaviour {
    public float shrinkTimeMin;
    public float shrinkTimeMax;
    public float shrinkBeginTime;
    public SpriteRenderer renderer;
    float shrinkStartTime;
    float shrinkTime;
    Vector2 originalScale;
    // Start is called before the first frame update
    void Start() {
        shrinkStartTime = Time.time+shrinkBeginTime;
        shrinkTime = Random.Range(shrinkTimeMin, shrinkTimeMax);
        originalScale = transform.localScale;
        float degrees = Random.Range(-360, 360);
        transform.rotation = Quaternion.Euler(0, 0, degrees);
        renderer.flipX = degrees < 0;
    }

    // Update is called once per frame
    void Update() {
        float time = Time.time - shrinkStartTime;
        if (time >= 0) {
            if (time >= shrinkTime) {
                Destroy(gameObject);
            } else {
                transform.localScale = originalScale * (1 - time / shrinkTime);
            }
        }
    }
}
