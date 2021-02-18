using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBushController : MonoBehaviour
{
    public int baseNoOfBushes;
    // Start is called before the first frame update
    void Start()
    {


        GameObject[] hectares = GameObject.FindGameObjectsWithTag("Hectare");
        int totalBushes = 0;
        System.Random randomValue = new System.Random();

        foreach (GameObject hect in hectares)
        {
            // hect.transform.parent = ga.transform;

          
            int bushesToSpawn = (int) (baseNoOfBushes * randomValue.NextDouble());
            totalBushes += bushesToSpawn;
            float minimumX = 0.45F;
            float minimumZ = 0.45F;
            while(bushesToSpawn > 0)     
            {
                GameObject fruitBush = (GameObject)Instantiate(Resources.Load("FruitBush"), new Vector3(randomValue.Next(-120, 120), 1F, randomValue.Next(-120, 120)), Quaternion.identity);
                fruitBush.transform.parent = hect.transform;
                fruitBush.transform.localPosition = new Vector3(GenerateRandomFloat(-minimumX, minimumX, randomValue), 0F, GenerateRandomFloat(-minimumZ, minimumZ, randomValue));                               
                MeshRenderer renderer = fruitBush.GetComponent<MeshRenderer>();
                Color colour = renderer.material.color;
                colour.a = 0.5f;
                renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                fruitBush.tag = "FruitBush";
                FruitBush fruitBushScript = fruitBush.GetComponent(typeof(FruitBush)) as FruitBush;
                fruitBushScript.fruitBush = fruitBush;
                bushesToSpawn--;

            }
            
        }
        Debug.Log(totalBushes);

                    


    }

    float GenerateRandomFloat(float minimumValue, float maximumValue, System.Random randomPoint)
    {
        float min = -0.5F + (float) (maximumValue - minimumValue) *  (float) randomPoint.NextDouble();
        return min;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
