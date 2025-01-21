using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    float start_y;
    float start_z;
    public GameObject player;
    public float yMinNoMoveCam;
    public float yMaxNoMoveCam;
    public float xMinNoMoveCam;
    public float xMaxNoMoveCam;
    public float yOffset;
    public float speed;
    public float YLowest;
    // Start is called before the first frame update
    void Start()
    {
        start_y = transform.position.y;
        start_z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 campos = new Vector3 (transform.position.x, transform.position.y - yOffset, transform.position.z);
        Vector3 relativePos = player.transform.position - transform.position;
        relativePos.z = 0;
        //Debug.Log(relativePos);
        if (relativePos.x > xMaxNoMoveCam)
        {
            transform.position = new Vector3(transform.position.x + relativePos.x-xMaxNoMoveCam , transform.position.y, transform.position.z);
        } else if (relativePos.x < xMinNoMoveCam) {
            transform.position = new Vector3(transform.position.x + relativePos.x-xMinNoMoveCam , transform.position.y, transform.position.z);
        }
        if (relativePos.y > yMaxNoMoveCam)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y + relativePos.y - yMaxNoMoveCam, YLowest), transform.position.z);
        } else if (relativePos.y < yMinNoMoveCam) {
            transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y + relativePos.y - yMinNoMoveCam, YLowest), transform.position.z);
        }
    }
}

