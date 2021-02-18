using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBush : MonoBehaviour
{
    public GameObject fruitBush;
    ArrayList fruitContained = new ArrayList();
    // Start is called before the first frame update
    void Start()
    {
        System.Random random = new System.Random();
        for (int i = 0; i < 20; i++)
        {

            GameObject fruit = (GameObject)Instantiate(Resources.Load("Fruit"), Random.insideUnitSphere + fruitBush.transform.position, Quaternion.identity);
            fruit.transform.localPosition = new Vector3(fruit.transform.localPosition.x, fruit.transform.localPosition.y + 1F, fruit.transform.localPosition.z) ;
            fruit.transform.parent = fruitBush.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
