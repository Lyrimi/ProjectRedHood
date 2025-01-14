using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCode : MonoBehaviour
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TextDisplay.SetActive(true);
            collision.gameObject.SendMessage("SetIsHousePresent", true);
            collision.gameObject.SendMessage("SetCurrentSign", gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TextDisplay.SetActive(false);
            collision.gameObject.SendMessage("SetIsHousePresent", false);
        }
    }
}
