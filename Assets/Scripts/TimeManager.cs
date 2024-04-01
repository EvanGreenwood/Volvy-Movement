using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
   
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale = 1;
            }
            else  
            {
                Time.timeScale = 0.15f;
            }
        } 
    }
}
