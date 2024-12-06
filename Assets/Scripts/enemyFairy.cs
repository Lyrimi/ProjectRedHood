using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class enemyFairy : MonoBehaviour
{
    //The Fairys sprite renderer. The sprite renderer is, by default, on a different, dedicated object.
    public SpriteRenderer sr;
    //The enemy of the Fairy. By default, it is supposed to be the player.
    public GameObject enemy;

    //The speed of the Fairy.
    public float speed;

    //The range of the zone which will make the Fairy angry if its enemy enters it.
    public float angerXRange;
    public float angerAboveRange;
    public float angerBelowRange;
    //The range of the zone which will calm the Fairy down if its enemy exits it.
    public float calmXRange;
    public float calmAboveRange;
    public float calmBelowRange;

    //Cycle behaviour related variables.
    //cycleReverseMin/MaxTime control the min and the max time in frames between when the Fairy reverses cycling directions.
    public int cycleReverseMinTime;
    public int cycleReverseMaxTime;
    //cycleAngleChange controls how much the cycling angle changes each frame.
    public float cycleAngleChange;

    //Wander behaviour related variables.
    //wanderAngleMin/MaxTime controls the min and max time in frames between each time the fairy changes its target wander angle.
    public int wanderAngleMinTime;
    public int wanderAngleMaxTime;
    //wanderAngleChangeSpeed controls how quickly the Fairy adjusts its current wander angle towards the target wander angle, in radians per frame.
    public float wanderAngleChangeSpeed;

    //Circling behaviour related variables.
    //circlingAngleMin/MaxTime controls the min and max time in frames between each time the fairy changes its target desired circling angle.
    public int circlingAngleMinTime;
    public int circlingAngleMaxTime;
    //circlingAngleMin/Max controls the min and max angle the Fairy will desire to be at relative to its enemy, measured in radians, where directly below the enemy is 0 radians.
    public float circlingAngleMin;
    public float circlingAngleMax;
    //circlingAngleChangeSpeed controls how quickly the Fairys current desired circling angle will change towards the target desired circling angle, in radians per frame.
    public float circlingAngleChangeSpeed;

    //The exact distance from the enemy the Fairy will try to stay at.
    public float preferredDistance;
    
    //Weights for the different behaviours.
    //The behaviours will generate a vector which represents how they would like to move (some unit vectors, some not).
    //The generated vectors will be multiplied by their weight, then added together, and finally the sum is normalized and multiplied by speed.
    public float cycleWeight;
    public float homeSeekingWeight;
    public float targetSeekingWeight;
    public float wanderWeight;
    public float circlingWeight;

    //If the movement vector has a length below this threshold, it will not be scaled all the way to speed, but will be shorter, in order to prevent janky motion when the behaviour vectors almost cancel each other out completely.
    public float lowSpeedThreshold;

    //Projectile related variables.
    //projectile is, surprisingly enough, the GameObject that the Fairy will fire as a projectile. The Fairy expects the projectile to have a SetDirection(Vector2 direction) function to set the direction it's fired at, and a SetIsAlly(bool ally) function to set whether the projectile is fired by an ally (it's not).
    public GameObject projectile;
    //The speed at which the projectile is fired at.
    public float projectileSpeed;
    //The delay between shots in a burst of shots.
    public int burstProjectileDelay;
    //The time from the Fairy getting angry 'til its first projectile burst.
    public int angerToBurstDelay;
    //burstMin/Max controls the min and max amount of projectiles in a burst.
    public int burstMin;
    public int burstMax;
    //burstTimeMin/MaxBase controls the min and max base amount of time between projectile bursts, while burstTimeMin/MaxPer controls how much time is added for each projectile of the burst.
    public int burstTimeMinBase;
    public int burstTimeMaxBase;
    public int burstTimeMinPer;
    public int burstTimeMaxPer;
    
    //Rigidbody and collider of the Fairy
    Rigidbody2D rb;
    Collider2D col;
    //Whether the Fairy is angry. When it's angry, it will chase the target and fire at it, while, when it's calm, it will wander around near its home.
    Boolean angry = false;
    //The Fairys home. It will move towards this when calm.
    Vector2 home;

    //Working values of the cycle behaviour.
    //cycleAngle is the current angle the cycling behaviour will move at.
    float cycleAngle;
    //cycleReversed stores whether the cycling is currently in the reversed direction.
    Boolean cycleReversed;
    //cycleReverseTime stores how long until the cycle direction reverses again.
    int cycleReverseTime;

    //Working values of the wander behaviour. Only used while calm.
    //wanderAngleCurrent is the angle the Fairy is currently moving, and wanderAngleTarget is the angle that wanderAngleCurrent moves towards.
    float wanderAngleCurrent;
    float wanderAngleTarget;
    //wanderAngleTime stores how long until a new wanderAngleTarget is picked, which wanderAngleCurrent will move towards.
    int wanderAngleTime;

    //Working values of the circling behaviour. Only used while angry.
    //circlingAngleCurrent is the angle the Fairy would currently like to stay at relative to its enemy, and circlingAngleTarget is the angle that circlingAngleCurrent moves toward.
    float circlingAngleCurrent;
    float circlingAngleTarget;
    //circlingAngleTime is the time until a new circlingAngleTarget is picked, which circlingAngleCurrent will move towards.
    float circlingAngleTime;
    
    //Working values of projectile bursts. Only used while angry.
    //burstTime is the time until the Fairy will shoot a new projectile burst.
    int burstTime;
    //burstSize is the amount of remaining projectiles in the current burst. Is 0 if the Fairy is not currently firing.
    int burstSize;
    //burstCurrentDelay is the current delay between the shots in a burst.
    int burstCurrentDelay;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch rigidbody and collider.
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        //Set home equal to spawn point.
        home = transform.position;

        //Set up cycle behaviour variables.
        cycleReverseTime = Random.Range(cycleReverseMinTime, cycleReverseMaxTime+1);
        cycleAngle = Random.Range(0, Mathf.PI*2);
        //Set up wander behaviour variables.
        wanderAngleTarget = Random.Range(0, Mathf.PI*2);
        wanderAngleCurrent = wanderAngleTarget;
        wanderAngleTime = Random.Range(wanderAngleMinTime, wanderAngleMaxTime+1);
        //Variables for the other behaviours don't need to be set up, as they either don't have variables that need to be set up, or are angry behaviours. Angry behaviours don't need to be set up at start, as the Fairy starts calm, and sets up the angry behaviour variables when switching to angry anyway.
    }

    void FixedUpdate()
    {
        //Create a vector with the distance between the fairy and its enemy
        Vector2 distance = enemy.transform.position-transform.position;

        if (!angry) {
            //If the Fairy is not angly, check if the target is inside the Fairys anger range, and switch it to angry if it is.
            if (Mathf.Abs(distance.x) <= angerXRange && distance.y <= angerAboveRange && distance.y >= -angerBelowRange) {
                angry = true;
                //Change Fairy sprite to angry. Currently just colors the box red.
                sr.color = new Color(255, 0, 0);

                //Set up circling behaviour related variables.
                //Set the desired circling angle target to a random value in the range, and set the current desired circling angle to already be equal to it.
                circlingAngleTarget = Random.Range(circlingAngleMin, circlingAngleMax);
                circlingAngleCurrent = circlingAngleTarget;
                //Reset the circling angle timer.
                circlingAngleTime = Random.Range(circlingAngleMinTime, circlingAngleMaxTime+1);
                
                //Set the time-'til-projectile-burst timer to the variable controlling what the timer should be set to when the Fairy enters anger mode.
                burstTime = angerToBurstDelay;
            }
        } else {
            //If the Fairy is angry, check if the target has exited the Fairys calm zone, and switch it to calm if it has.
            if (Mathf.Abs(distance.x) > calmXRange || distance.y > calmAboveRange || distance.y < -calmBelowRange) {
                angry = false;
                //Change Fairy sprite to calm. Currently just colors the box white.
                sr.color = new Color(255, 255, 255);

                //Set up wander behaviour related variables.
                //The wander target angle is set to a random angle, and the current wander angle is set to already be equal to it.
                wanderAngleTarget = Random.Range(0, Mathf.PI*2);
                wanderAngleCurrent = wanderAngleTarget;
                //Reset the wander angle timer.
                wanderAngleTime = Random.Range(wanderAngleMinTime, wanderAngleMaxTime+1);
            }
        }

        //Change the cycleAngle. The direction depends on if the cycle is currently reversed or not.
        cycleAngle += cycleAngleChange*(cycleReversed ? -1 : 1);
        //Make sure the angle value stays within 0 (inclusive) and tau (exclusive) by looping it back if its outside.
        //Skip checking the bound in the direction it's not currently changing, as thats unnecessary.
        if (!cycleReversed && cycleAngle >= Mathf.PI * 2) {
            cycleAngle -= Mathf.PI*2;
        } else if (cycleReversed && cycleAngle < 0) {
            cycleAngle += Mathf.PI*2;
        }

        //Cycle Reverse timer countdown.
        cycleReverseTime--;
        if (cycleReverseTime == 0) {
            //If the cycle reverse timer has counted down, reverse the cycle and restart the timer.
            cycleReversed = !cycleReversed;
            cycleReverseTime = Random.Range(cycleReverseMinTime, cycleReverseMaxTime+1);
        }

        //Vectors for the three behaviours.
        Vector2 cycleVector = new Vector2(Mathf.Cos(cycleAngle), Mathf.Sin(cycleAngle))*cycleWeight;
        Vector2 distanceVector;
        Vector2 wanderVector;

        if (!angry) {
            //If the current wander angle isn't the target wander angle, rotate it towards it.
            if (wanderAngleCurrent != wanderAngleTarget) {
                //Find the smallest difference between the current and the target angle.
                float wanderAngleDiff = wanderAngleTarget-wanderAngleCurrent;
                if (wanderAngleDiff < -Mathf.PI) {
                    wanderAngleDiff += Mathf.PI*2;
                } else if (wanderAngleDiff >= Mathf.PI) {
                    wanderAngleDiff -= Mathf.PI*2;
                }

                //If the next rotation of current angle would overshoot the target angle, then just skip to the target angle, otherwise, rotate it as normal (in the shortest direction by using wanderAngleDiff's sign).
                if (Mathf.Abs(wanderAngleDiff) < wanderAngleChangeSpeed) {
                    wanderAngleCurrent = wanderAngleTarget;
                } else {
                    wanderAngleCurrent += wanderAngleChangeSpeed*Mathf.Sign(wanderAngleDiff);
                }
            }

            //Wander Angle timer countdown.
            wanderAngleTime--;
            if (wanderAngleTime == 0) {
                //If the wander angle timer has counted down, pick a new wander angle target and reset the timer.
                wanderAngleTarget = Random.Range(0, Mathf.PI*2);
                wanderAngleTime = Random.Range(wanderAngleMinTime, wanderAngleMaxTime+1);
            }

            //Distance vector points towards the home position, and is scaled by homeSeekingWeight.
            distanceVector = new Vector2(home.x-transform.position.x, home.y-transform.position.y)*homeSeekingWeight;
            //Wander vector simply converts the current wander angle to a unit vector pointing in that direction, then scales it by wanderWeight.
            wanderVector = new Vector2(Mathf.Cos(wanderAngleCurrent), Mathf.Sin(wanderAngleCurrent))*wanderWeight;
        } else {
            //If currently doing a projectile burst (if there's still projectiles from the burst left to be fired)
            if (burstSize > 0) {
                //If it's time to shoot another projectile of the burst.
                if (burstCurrentDelay == 0) {
                    //Create a new instance of the projectile object.
                    GameObject projectile = Instantiate(this.projectile, transform.position, transform.rotation);
                    //Setup the projectile to be an enemy projectile, and set the launch direction and speed by taking the distance vector, which points from the fairy to its enemy, normalize it, and multiply it by the projectile speed.
                    projectile.SendMessage("setDirection", distance.normalized*projectileSpeed);
                    projectile.SendMessage("setIsAlly", false);
                    
                    //Reset the time-between-shots-in-the-burst timer, and decrement the burst shots counter.
                    burstCurrentDelay = burstProjectileDelay;
                    burstSize--;
                } else {
                    //Time-between-shots-in-the-burst timer countdown.
                    burstCurrentDelay--;
                }
            } else {
                //If it's time to shoot a projectile burst, pick a random burst projectile count (burstSize), and reset the timer, with min and max timer values affected by burst size.
                if (burstTime == 0) {
                    burstSize = Random.Range(burstMin, burstMax+1);
                    burstTime = Random.Range(burstTimeMinBase+burstTimeMinPer*burstSize, burstTimeMaxBase+burstTimeMaxPer*burstSize+1);
                } else {
                    //Time-'til-burst timer countdown.
                    burstTime--;
                }
            }

            //If the current desired circling angle isn't the same as the target desired circling angle, move the current desired value towards the target desired value.
            //The desired angle value is the angle that the fairy would like to be at compared to the target, measured in radians, where 0 radians is below the target.
            if (circlingAngleCurrent != circlingAngleTarget) {
                //If rotating the desired current angle as normal would overshoot the target desired angle, just set the angle to the target.
                if (Mathf.Abs(circlingAngleTarget-circlingAngleCurrent) < circlingAngleChangeSpeed) {
                    circlingAngleCurrent = circlingAngleTarget;
                } else {
                    //Otherwise, rotate it as normal.
                    circlingAngleCurrent += circlingAngleChangeSpeed*Mathf.Sign(circlingAngleCurrent > circlingAngleTarget ? -1 : 1);
                }
            }

            //The angle between the Fairy and its enemy, measured in radians, where 0 radians means the Fairy is below its enemy.
            //Calculated by taking the distance vector, which points from the fairy to the enemy, inverting it, so it points from the enemy to the fairy, then rotating it by 90 degrees.
            //  The transformed vector is equivalent to a vector with x equal to distance.y, and y equal to -distance.x
            float circlingAngleCurrentAngle = Mathf.Atan2(-distance.x, distance.y);
            //The difference between the current desired angle, and the actual current angle. the absolute value of the desired angle is taken, so the left and right hand side of the enemy is mirrored when it comes to the angles.
            float circlingAngleDiff = circlingAngleCurrent-Mathf.Abs(circlingAngleCurrentAngle);
            
            //Circling angle timer countdown.
            circlingAngleTime--;
            if (circlingAngleTime == 0) {
                //If the circling angle timer has counted down, then reset it and pick a new circling desired angle target.
                circlingAngleTarget = Random.Range(circlingAngleMin, circlingAngleMax);
                circlingAngleTime = Random.Range(circlingAngleMinTime, circlingAngleMaxTime+1);
            }

            //The distance vector is set to point towards being a certain distance from the fairys enemy, and is weighted by the target seeking weight.
            distanceVector = (distance.magnitude-preferredDistance)*distance.normalized*targetSeekingWeight;
            //The wander vector is set to point towards the direction the Fairy has to move in order to get the current desired angle between itself and its enemy. The vector is set to be tangential to the vector pointing from the fairy to the enemy.
            //  The wander vector is of course also scaled by the circling weight.
            //The wander vector is calculated by taking the inverse distance vector rotated by 90 degrees and normalized, multiplying it with the difference in current angle and current desired angle, causing it to have the correct sign and a magnitude proportional to the distance between the current angle and the desired angle, and finally multiplying with the sign of the angle between the Fairy and its enemy, so the vector gets correctly mirrored if the Fairy is on the left-hand side of the enemy.
            wanderVector = new Vector2(distance.y, -distance.x).normalized*circlingAngleDiff*Mathf.Sign(circlingAngleCurrentAngle)*circlingWeight;
        }
        //Finally, the vectors from the different behaviours are added together.
        Vector2 finalVector = cycleVector+distanceVector+wanderVector;
        //Now the final vector is supposed to be normalized, but if it's shorter than lowSpeedThreshold, it will instead be scaled to a smaller length than 1 instead, to prevent janky motion when the behaviour vectors almost cancel out to zero.
        if (finalVector.magnitude < lowSpeedThreshold) {
            finalVector /= lowSpeedThreshold;
        } else {
            //If it wasn't too short, just normalize it as normal.
            finalVector = finalVector.normalized;
        }
        //Scale the final vector up by the speed value.
        finalVector *= speed;
        //The final scaled behaviour vector is then directly set to be the Fairys velocity, which is not the way you are supposed to do it, but I don't know how to control AddForce well enough, and this works well enough.
        //  This will make the Fairy react incorrectly to outside forces, but this likely wont be a problem in this project, and so it's not worth spending a lot of effort fixing. The only difference this is likely to really make, is that the Fairy wont be forced down correctly if something falls down on its head and pushes it down by gravity.
        rb.velocity = finalVector;
    }
}
