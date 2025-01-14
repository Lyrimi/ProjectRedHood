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
    public float yOffset;
    public float speed;
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
        Debug.Log(relativePos);
        if (math.abs(relativePos.x) > xNoMoveCam)
        {
            transform.position = new Vector3(transform.position.x + (math.abs(relativePos.x)-xNoMoveCam)*math.clamp(relativePos.x, -1, 1) , transform.position.y, transform.position.z);
        }
        if (math.abs(relativePos.y) > xNoMoveCam)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (math.abs(relativePos.y) - xNoMoveCam) * math.clamp(relativePos.y, -1, 1), transform.position.z);
        }
    }
}

