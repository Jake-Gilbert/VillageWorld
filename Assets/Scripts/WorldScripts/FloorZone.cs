using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorZone : MonoBehaviour
{
    public int fruitCount = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Fruit"))
        {
            fruitCount++;
        }
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
