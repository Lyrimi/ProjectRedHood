using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SignDisplay : MonoBehaviour
{
    public GameObject TextDisplay;
    public string Content;
    public Dialog dialog;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void GetContent()
    {
        dialog.TypeWriter(Content, 0.05f, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TextDisplay.SetActive(true);
            collision.gameObject.SendMessage("SetIsSignPresent", true);
            collision.gameObject.SendMessage("SetCurrentSign", gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TextDisplay.SetActive(false);
            collision.gameObject.SendMessage("SetIsSignPresent", false);
        }
    }
}
