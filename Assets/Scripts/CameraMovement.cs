using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    float start_y;
    float start_z;
    public GameObject player;
    public float yNoMoveCam;
    public float xNoMoveCam;
    // Start is called before the first frame update
    void Start()
    {
        start_y = transform.position.y;
        start_z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePos = player.transform.position - transform.position;
        relativePos.z = 0;
        Debug.Log(relativePos);
        if (math.abs(relativePos.x) > xNoMoveCam)
        {
            transform.position = transform.position + relativePos*Time.deltaTime;
        }
    }
}

