using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public GameObject viewer;
    public GameObject message;
    public GameObject cam;
    public Vector2 messageOrigin;
    public Vector2 messagePosition;
    public Vector2 messageScale;
    public float displaySpeed;
    public float desplaySpeed;
    public Vector2 viewTriggerCenter;
    public Vector2 viewTriggerSize;

    float display = 0;

    void Start() {
        UpdateDisplay();
    }

    void Update() {
        bool update = false;
        Vector2 distance = viewer.transform.position-(Vector3)viewTriggerCenter-transform.position;
        if (Mathf.Abs(distance.x) <= viewTriggerSize.x/2 && Mathf.Abs(distance.y) <= viewTriggerSize.y/2) {
            if (display != 1) {
                display += displaySpeed*Time.deltaTime;
                if (display > 1) {
                    display = 1;
                }
            }
            update = true;
        } else {
            if (display != 0) {
                display -= desplaySpeed*Time.deltaTime;
                update = true;
                if (display < 0) {
                    display = 0;
                }
            }
        }
        if (update) {
            UpdateDisplay();
        }
    }

    void UpdateDisplay() {
        message.transform.position = (messageOrigin+(Vector2) transform.position)*(1-display)+(messagePosition+(Vector2) cam.transform.position)*display;
        message.transform.localScale = messageScale*display;
        message.SetActive(display != 0);
    }
}
