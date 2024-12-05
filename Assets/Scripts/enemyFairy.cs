using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class enemyFairy : MonoBehaviour
{
    public SpriteRenderer sr;
    public GameObject enemy;

    public float speed;

    public float angerXRange;
    public float angerAboveRange;
    public float angerBelowRange;
    public float calmXRange;
    public float calmAboveRange;
    public float calmBelowRange;

    public int cycleReverseMinTime;
    public int cycleReverseMaxTime;
    public float cycleAngleChange;

    public int wanderAngleMinTime;
    public int wanderAngleMaxTime;
    public int wanderAngleTimeAngerDrop;
    public float wanderAngleChangeSpeed;

    public int circlingAngleMinTime;
    public int circlingAngleMaxTime;
    public float circlingAngleMin;
    public float circlingAngleMax;
    public float circlingAngleChangeSpeed;

    public float preferredDistance;
    
    public float cycleWeight;
    public float homeSeekingWeight;
    public float targetSeekingWeight;
    public float wanderWeight;
    public float circlingWeight;

    Rigidbody2D rb;
    Boolean angry = false;
    Vector2 home;

    float cycleAngle;
    Boolean cycleReversed;
    int cycleReverseTime;

    float wanderAngleCurrent;
    float wanderAngleTarget;
    int wanderAngleTime;

    float circlingAngleCurrent;
    float circlingAngleTarget;
    float circlingAngleTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        home = transform.position;
        cycleReverseTime = Random.Range(cycleReverseMinTime, cycleReverseMaxTime);
        cycleAngle = Random.Range(0, Mathf.PI*2);
        wanderAngleTarget = Random.Range(0, Mathf.PI*2);
        wanderAngleCurrent = wanderAngleTarget;
        wanderAngleTime = Random.Range(wanderAngleMinTime, wanderAngleMaxTime);
    }

    void FixedUpdate()
    {
        Vector2 distance = enemy.transform.position-transform.position;
        if (!angry) {
            if (Mathf.Abs(distance.x) <= angerXRange && distance.y <= angerAboveRange && distance.y >= -angerBelowRange) {
                angry = true;
                sr.color = new Color(255, 0, 0);

                wanderAngleTime -= wanderAngleTimeAngerDrop;
                if (wanderAngleTime <= 0) {
                    wanderAngleTarget = Random.Range(0, Mathf.PI*2);
                    wanderAngleTime = Random.Range(wanderAngleMinTime, wanderAngleMaxTime);
                }

                circlingAngleTarget = Random.Range(circlingAngleMin, circlingAngleMax);
                circlingAngleCurrent = circlingAngleTarget;
                circlingAngleTime = Random.Range(circlingAngleMinTime, circlingAngleMaxTime);
            }
        } else {
            if (Mathf.Abs(distance.x) > calmXRange || distance.y > calmAboveRange || distance.y < -calmBelowRange) {
                angry = false;
                sr.color = new Color(255, 255, 255);
            }
        }

        cycleAngle += cycleAngleChange*(cycleReversed ? -1 : 1);
        if (!cycleReversed && cycleAngle >= Mathf.PI * 2) {
            cycleAngle -= Mathf.PI*2;
        } else if (cycleReversed && cycleAngle < 0) {
            cycleAngle += Mathf.PI*2;
        }

        cycleReverseTime--;
        if (cycleReverseTime == 0) {
            cycleReversed = !cycleReversed;
            cycleReverseTime = Random.Range(cycleReverseMinTime, cycleReverseMaxTime);
        }

        Vector2 cycleVector = new Vector2(Mathf.Cos(cycleAngle), Mathf.Sin(cycleAngle))*cycleWeight;
        Vector2 distanceVector;
        Vector2 wanderVector;
        if (!angry) {
            if (wanderAngleCurrent != wanderAngleTarget) {
                float wanderAngleDiff = wanderAngleTarget-wanderAngleCurrent;
                if (wanderAngleDiff < -Mathf.PI) {
                    wanderAngleDiff += Mathf.PI*2;
                } else if (wanderAngleDiff >= Mathf.PI) {
                    wanderAngleDiff -= Mathf.PI*2;
                }
                if (Mathf.Abs(wanderAngleDiff) < wanderAngleChangeSpeed) {
                    wanderAngleCurrent = wanderAngleTarget;
                } else {
                    wanderAngleCurrent += wanderAngleChangeSpeed*Mathf.Sign(wanderAngleDiff);
                }
            }

            wanderAngleTime--;
            if (wanderAngleTime == 0) {
                wanderAngleTarget = Random.Range(0, Mathf.PI*2);
                wanderAngleTime = Random.Range(wanderAngleMinTime, wanderAngleMaxTime);
            }

            distanceVector = new Vector2(home.x-transform.position.x, home.y-transform.position.y)*homeSeekingWeight;
            wanderVector = new Vector2(Mathf.Cos(wanderAngleCurrent), Mathf.Sin(wanderAngleCurrent))*wanderWeight;
        } else {
            if (circlingAngleCurrent != circlingAngleTarget) {
                if (Mathf.Abs(circlingAngleTarget-circlingAngleCurrent) < circlingAngleChangeSpeed) {
                    circlingAngleCurrent = circlingAngleTarget;
                } else {
                    circlingAngleCurrent += circlingAngleChangeSpeed*Mathf.Sign(circlingAngleCurrent > circlingAngleTarget ? -1 : 1);
                }
            }

            float circlingAngleCurrentAngle = Mathf.Atan2(-distance.x, distance.y);
            float circlingAngleDiff = circlingAngleCurrent-Mathf.Abs(circlingAngleCurrentAngle);
            
            circlingAngleTime--;
            if (circlingAngleTime == 0) {
                circlingAngleTarget = Random.Range(circlingAngleMin, circlingAngleMax);
                circlingAngleTime = Random.Range(circlingAngleMinTime, circlingAngleMaxTime);
            }

            // ## Algebra! ##
            // distance = d, distance.normalized = n, preferredDistance = p, distance.magnitude = m, targetSeekingWeight = t
            // -n*(p-m)*t
            // -d/m*(p-m)*t
            // -d*(p/m-m/m)*t
            // -d*(p/m-1)*t
            // d*(1-p/m)*t
            // (1-p/m)*d*t
            distanceVector = (1-preferredDistance/distance.magnitude)*distance*targetSeekingWeight;
            wanderVector = new Vector2(distance.y, -distance.x).normalized*circlingAngleDiff*Mathf.Sign(circlingAngleCurrentAngle)*circlingWeight;
        }
        rb.velocity = (cycleVector+distanceVector+wanderVector).normalized*speed;
    }
}
