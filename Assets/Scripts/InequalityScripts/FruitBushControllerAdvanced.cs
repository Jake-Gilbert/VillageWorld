using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FruitBushControllerAdvanced : FruitBushController
{
    public List<GameObject> depletedBushes = new List<GameObject>();

    private void Start()
    {
        ProduceNewBushes(12);
    }

    public void ReplenishBushes()
    {
        int bushesToReplenish = (int) (depletedBushes.Count * (Random.Range(0.3F, 0.9F)));
        List<GameObject> indexesToRemove = new List<GameObject>();
        for (int i = 0; i < bushesToReplenish; i++)
        {
            if (depletedBushes[i] == null)
            {
                indexesToRemove.Add(depletedBushes[i]);
            }
            else
            {
                depletedBushes[i].SetActive(true);
                depletedBushes[i].GetComponent<FruitBush>().SpawnFruit();
                indexesToRemove.Add(depletedBushes[i]);
            }
        }
        depletedBushes.Except(indexesToRemove);      
    }

    public void ProduceNewBushes(int hectaresToUse)
    {
        GameObject[] hectares = GameObject.FindGameObjectsWithTag("Hectare");
        for (int i = 0; i < hectaresToUse; i++)
        {
            int bushesToSpawn = (int)(baseNoOfBushes * Random.value + 1);
            float boundary = 0.5F;
            while (bushesToSpawn > 0)
            {
                GameObject fruitBush = (GameObject)Instantiate(Resources.Load("FruitBush"), new Vector3(Random.Range(-120, 120), 1F, Random.Range(-120, 120)), Quaternion.identity);
                fruitBush.transform.parent = hectares[i].transform;
                fruitBush.transform.localPosition = new Vector3(Random.Range(-boundary, boundary), 0F, Random.Range(-boundary, boundary));
                fruitBush.transform.parent = null;
                MeshRenderer renderer = fruitBush.GetComponent<MeshRenderer>();
                fruitBush.tag = "FruitBush";
                bushesToSpawn--;
            }
        }
    }

}
