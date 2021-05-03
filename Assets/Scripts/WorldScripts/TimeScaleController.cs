using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : MonoBehaviour
{    void Update()
    {
        if (Input.GetKeyDown(KeyCode.AltGr)) 
        {
            if (Time.timeScale!= 0)
            {
                Time.timeScale -= 0.25F;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.RightControl)) 
        {
            if (Time.timeScale < 4.25F)
            {
                Time.timeScale += 1F;
            }
        }
    }
}
