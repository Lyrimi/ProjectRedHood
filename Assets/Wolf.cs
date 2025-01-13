using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Wolf : EntityBase
{
    public GameObject Spike;
    public GameObject Slash;
    public Transform SlashSpawnpoint;
    Animator anim;
    public Healthbar healthbar;
   // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        healthbar.SetMaxHealth(MaxHealth);
        healthbar.Sethealth(MaxHealth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthbar.Sethealth(health);
    }

    public void summonSlash()
    {
        Vector2 pos = SlashSpawnpoint.position;
        Instantiate(Slash, pos, Quaternion.identity);
    }

    public void summonSpike()
    {
        Instantiate(Spike);
    }
    IEnumerator AnimatorWait(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        anim.SetBool("Wating", false);

    }
    public void SetWait(float waitime)
    {
        StartCoroutine(AnimatorWait(waitime));
    }

    public new void Death()
    {
        Destroy(healthbar.gameObject);
        base.Death();
    }
}