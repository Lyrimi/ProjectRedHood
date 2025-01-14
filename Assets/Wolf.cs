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
    public Dialog dialog;
    public GameManager gameManager;
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

    IEnumerator Victory()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Restarted");
        gameManager.nextScene("Start");

    }

    public new void Death()
    {
        StartCoroutine(Victory());
        dialog.TypeWriter("Congrats you win but....... Grandama is Still Dead", 0.05f, 2f);
        
        Destroy(healthbar.gameObject);
        base.Death();

    }
    
}