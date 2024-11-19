using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveing_platform : platform_colision
{
    Transform startpos;
    public Transform[] list;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
