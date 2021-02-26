using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBush : MonoBehaviour
{
    public GameObject bush;
    private SphereCollider collider;
    private int numberOfFruit = 0;
    private List<GameObject> fruits;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponentInChildren<SphereCollider>();
        fruits = new List<GameObject>();
        for (int i = 0; i < Random.Range(1, 6); i++)
        {
            GameObject rotationPoint = new GameObject();
            rotationPoint.transform.position = bush.transform.position;
            GameObject fruit = (GameObject)Instantiate(Resources.Load("Fruit"), bush.transform.position + new Vector3(0,  collider.radius * 2.9F, 0), Quaternion.identity);
            fruit.transform.parent = rotationPoint.transform;
            rotationPoint.transform.eulerAngles = new Vector3(Random.Range(-100,100), Random.Range(-50, 50), Random.Range(-100, 100));
            fruit.transform.parent = bush.transform;
            Destroy(rotationPoint.gameObject);
            numberOfFruit++;
            fruits.Add(fruit);
        }
    }

    public int GetTotalFruit()
    {
        return numberOfFruit;
    }

    private void Update()
    {
       
       if(fruits.Count != numberOfFruit)
        {
            numberOfFruit--;
        }
    }


}
