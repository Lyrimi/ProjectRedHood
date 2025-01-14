using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuControler : MonoBehaviour
{
    public InputManager InputManager;
    public GameObject text;
    public Camera cam;
    public GameManager gameManager;
    bool zoom;
    private void OnEnable()
    {
        InputManager.DashEvent += inputX;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (zoom)
        {
            cam.orthographicSize = cam.orthographicSize - 1f * Time.deltaTime;
        }
    }
    void inputX()
    {
        text.SetActive(false);
        cam.orthographicSize = 5;
        zoom = true;
        gameManager.nextScene("Start");
    }
}
