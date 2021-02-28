using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class FruitCollected : MonoBehaviour
{   
    GameObject floorZone;
    public TMP_Text fruitCollectedText;
    public int fruitCollected;

    // Start is called before the first frame update
    void Start()
    {
        fruitCollectedText = gameObject.GetComponent<TMP_Text>();

        fruitCollected = GetFruitCollected();
        if (fruitCollectedText != null)
        {
            fruitCollectedText.text = $"Fruit Collected: {GetFruitCollected()}";
        }

      
    }

    // Update is called once per frame
    void Update()
    {  
            if (fruitCollectedText.text == "")
            {
               fruitCollectedText.text = $"Fruit Collected: {GetFruitCollected()}";
            }
            if (GetFruitCollected() != fruitCollected)
            {
                fruitCollectedText.text = $"Fruit Collected: {GetFruitCollected()}";
                fruitCollected = GetFruitCollected();
            }     
    }

    int GetFruitCollected()
    {
        floorZone = GameObject.FindGameObjectWithTag("Zone");
        int total = floorZone.GetComponent<FloorZone>().GetFruitCount();       
        return total;
    }
     


      


}
