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

        foreach (GameObject hect in hectares)
        {          
            int bushesToSpawn = (int) (baseNoOfBushes * Random.value);
            totalBushes += bushesToSpawn;
            float boundary = 0.5F;
            while(bushesToSpawn > 0)     
            {
                GameObject fruitBush = (GameObject)Instantiate(Resources.Load("FruitBush"), new Vector3(Random.Range(-120, 120), 1F, Random.Range(-120, 120)), Quaternion.identity);
                fruitBush.transform.parent = hect.transform;
                fruitBush.transform.localPosition = new Vector3(Random.Range(-boundary, boundary), 0F, Random.Range(-boundary, boundary));                               
                MeshRenderer renderer = fruitBush.GetComponent<MeshRenderer>();
                Color colour = renderer.material.color;
                colour.a = 0.5f;
                renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                fruitBush.tag = "FruitBush";
                FruitBush fruitBushScript = fruitBush.GetComponent(typeof(FruitBush)) as FruitBush;
                //fruitBushScript.bush = fruitBush.transform.Find("Bush");
                fruitBushScript.bush = fruitBush.transform.GetChild(0).gameObject;
                bushesToSpawn--;

            }
            
        }
        Debug.Log(totalBushes);

                    


    }

}
