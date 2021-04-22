using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushRemover : MonoBehaviour
{
    float deathTimer = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameObject.transform.Find("Bush").gameObject.activeSelf && gameObject != null)
        {
            if (deathTimer > 30)
            {
                Destroy(gameObject);
            }
            deathTimer += Time.deltaTime;
        }
        else
        {
            deathTimer = 0;
        }
    }
}
