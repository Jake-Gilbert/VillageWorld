using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBushControllerAdvanced : FruitBushController
{
    public void ReplenishBushes()
    {
        int index = 0;

        GameObject[] hectares = GameObject.FindGameObjectsWithTag("Hectare");
        int totalBushes = 0;
        foreach (GameObject hect in hectares)
        {
            int bushesToSpawn = (int)(baseNoOfBushes * Random.value) / 2;
            totalBushes += bushesToSpawn;
            float boundary = 0.5F;
            while (bushesToSpawn > 0)
            {
                GameObject fruitBush = (GameObject)Instantiate(Resources.Load("FruitBush"), new Vector3(Random.Range(-120, 120), 1F, Random.Range(-120, 120)), Quaternion.identity);
                fruitBush.name = "FruitBush" + (index + 1);
                fruitBush.transform.parent = hect.transform;
                fruitBush.transform.localPosition = new Vector3(Random.Range(-boundary, boundary), 0F, Random.Range(-boundary, boundary));
                fruitBush.transform.parent = null;
                MeshRenderer renderer = fruitBush.GetComponent<MeshRenderer>();
                fruitBush.tag = "FruitBush";
                bushesToSpawn--;
                index++;
            }
         }
        return;
    }
}
