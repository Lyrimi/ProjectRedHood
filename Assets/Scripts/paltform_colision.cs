using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paltform_colision : MonoBehaviour
{
    public GameObject player;
    BoxCollider2D Box;
    // Start is called before the first frame update
    void Start()
    {
        Box = GetComponent<BoxCollider2D>();
        Box.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float playery = player.transform.position.y - (player.transform.localScale.y/2);
        if (playery >= transform.position.y)
        {
            Box.enabled = true;
          
        }
        else
        {
            Box.enabled = false;
        }
    }
}
