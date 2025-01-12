using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    bool waiting;
    // Start is called before the first frame update
    public void Stop(float stoptime)
    {
        if (waiting) { return;}
        Time.timeScale = 0;
        StartCoroutine(Wait(stoptime));
    }

    // Update is called once per frame
    IEnumerator Wait(float stoptime)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(stoptime);
        Time.timeScale = 1;
        waiting = false;
    }
}
