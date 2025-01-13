using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;

    public void Sethealth(int health)
    {

    slider.value = health; 

    }

    public void SetMaxHealth(int maxHealth)
    {
    slider.maxValue = maxHealth; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
