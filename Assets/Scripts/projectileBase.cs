using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public Vector2 direction;
    public bool ally;

    protected void handleIsAlly() {
        //Due to spaghetti, this runs twice with the current method of summoning the projectiles. I don't know how to fix it without possibly making more spaghetti. I'm sorry.
        Collider2D col = GetComponent<Collider2D>();
        col.excludeLayers |= LayerMask.GetMask(ally ? "Ally Entity" : "Enemy Entity");
    }

    public void setDirection(Vector2 direction) {
        this.direction = direction;
    }

    public void setIsAlly(bool ally) {
        this.ally = ally;
        handleIsAlly();
    }
}
