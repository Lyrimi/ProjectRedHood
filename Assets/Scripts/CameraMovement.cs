using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    float start_y;
    float start_z;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        start_y = transform.position.y;
        start_z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camerapos = player.transform.position;
        camerapos.y = start_y;
        camerapos.z = start_z;
        transform.position = camerapos;
    }
}

