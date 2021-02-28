﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FruitBush : MonoBehaviour
{
    public GameObject bush;
    private SphereCollider collider;
    private int numberOfFruit;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<SphereCollider>();
        int noOfFruits = Random.Range(1, 6);
        numberOfFruit = noOfFruits;
        for (int i = 0; i < noOfFruits; i++)
        {
            GameObject rotationPoint = new GameObject();
            rotationPoint.transform.position = bush.transform.position;
            GameObject fruit = (GameObject)Instantiate(Resources.Load("Fruit"), bush.transform.position + new Vector3(0,  collider.radius * 2.9F, 0), Quaternion.identity);
            fruit.transform.parent = rotationPoint.transform;
            rotationPoint.transform.eulerAngles = new Vector3(Random.Range(-100,100), Random.Range(-50, 50), Random.Range(-100, 100));
            fruit.transform.parent = bush.transform;
            Destroy(rotationPoint.gameObject);
        }
    }

    public int GetTotalFruit()
    {
        return numberOfFruit;
    }

    public void PickFruit()
    {
        if(numberOfFruit > 0)
        {
            GameObject fruit = gameObject.transform.GetChild(0).gameObject;
            if(fruit != null)
            {
                numberOfFruit--;
                Destroy(fruit);
            }          
           
        }
    }
    private void Update()
    {
        if(numberOfFruit <= 0)
        {
            Destroy(bush);
        }
      
    }


}
