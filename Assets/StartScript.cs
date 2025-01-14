using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    public Dialog dialog;
    // Start is called before the first frame update
    void Start()
    {
        dialog.TypeWriter("Arrow keys or WASD to move", 0.05f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
