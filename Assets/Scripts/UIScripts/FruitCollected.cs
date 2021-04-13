using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class FruitCollected : MonoBehaviour
{   
    private GameObject floorZone;
    public TMP_Text fruitCollectedText;
    public int fruitCollected;

    // Start is called before the first frame update
    private void Start()
    {
        fruitCollectedText = gameObject.GetComponent<TMP_Text>();

        fruitCollected = GetFruitCollected();
        if (fruitCollectedText != null)
        {
            fruitCollectedText.text = $"Fruit Collected: {GetFruitCollected()}";
        }

      
    }

    // Update is called once per frame
    private void Update()
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

    private int GetFruitCollected()
    {
        floorZone = GameObject.FindGameObjectWithTag("Zone");
        int total = 0;
        if (floorZone != null)
        {
            total = floorZone.GetComponent<FloorZoneAdvanced>().GetFruitCount();
        }
        return total;
    }
     


      


}
