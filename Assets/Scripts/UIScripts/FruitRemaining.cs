﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FruitRemaining : MonoBehaviour
{
    private GameObject[] fruitBushes;
    public TMP_Text fruitRemainingText;
    public int fruitRemaining;
   
    void Start()
    { 
        fruitRemainingText = gameObject.GetComponent<TMP_Text>();
        fruitRemaining = GetTotalFruit();
        if (fruitRemainingText != null)
        {
            fruitRemainingText.text = $"Fruit Remaining: {GetTotalFruit()}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fruitRemainingText.text == "")
        {
            fruitRemainingText.text = $"Fruit Remaining: {GetTotalFruit()}";
        }
        if (GetTotalFruit() != fruitRemaining)
        {
            fruitRemainingText.text = $"Fruit Remaining: {GetTotalFruit()}";
            fruitRemaining = GetTotalFruit();
        }
    }

    private int GetTotalFruit()
    {
        fruitBushes = GameObject.FindGameObjectsWithTag("Bush");

        int total = 0;

        foreach (GameObject bush in fruitBushes)
        {
            total += bush.GetComponent<FruitBush>().GetTotalFruit();            
        }
        return total;
    }
}
