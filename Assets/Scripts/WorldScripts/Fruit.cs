using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    bool isCarried;
    public bool pickable;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("FruitBush"))
        {
            this.tag = "TouchingBush";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        pickable = false;
        isCarried = false;
    }

    // Update is called once per frame
    void Update()
    { 
        
    }

    void onTriggerEnter(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(gameObject);
        }
    }
}
