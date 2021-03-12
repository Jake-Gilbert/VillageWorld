using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;



public class GiniCalculator : MonoBehaviour
{
    public TMP_Text fruitCollectedText;
    public FruitCollected fruitCollected;
    public List<float> giniValues;
    private int villagerCount = 0;
    float timer = 0;

    void Start()
    {
        giniValues = new List<float>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 5)
        {
            //Debug.Log(CalculateGiniCoefficient());
            giniValues.Add(CalculateGiniCoefficient());
            //CalculateGiniCoefficient();
            timer = 0;
        }
    }

    // Update is called once per frame
    float CalculateGiniCoefficient()
    {
        float[] scores = getVillagerScores();
        Debug.Log(string.Join(",", scores));
        float giniValue = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            giniValue += scores[i];
        }
        return 1 - giniValue;
    }

    float[] getVillagerScores()
    {
        if (villagerCount == 0)
        {
            villagerCount = GameObject.FindGameObjectsWithTag("Villager").Length;
        }

        //GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        List<GameObject> tempVillagers = GameObject.FindGameObjectsWithTag("Villager").ToList();
        float[] percentiles = new float[] { 0.02F, 0.18F, 0.3F, 0.5F };
        float[] scores = new float[4];

        if (tempVillagers != null)
        {
            float fruitThreshold = 0;
            float cumulativeFraction = 0;
            List<GameObject> villagersInThisPopulation = new List<GameObject>();
            List<GameObject> excludedPopulation = new List<GameObject>();
            for (int i = 0; i < 4; i++)
            {
                if (fruitCollected.fruitCollected > 0)
                {
                    fruitThreshold = percentiles[i] * fruitCollected.fruitCollected;
                }    

                int localVillagerCounter = 0;
        
                foreach (GameObject villager in tempVillagers)
                {
                    if (villager.GetComponent<AgentVillager1>().getFruitCollected() <= fruitThreshold)
                    {
                        localVillagerCounter++;
                        if (!excludedPopulation.Contains(villager))
                        {
                            villagersInThisPopulation.Add(villager);
                            excludedPopulation.Add(villager);
                        }
                        //villagersToDelete.Add(villager);
                    }
                }
                float fractionOfPopulation = villagersInThisPopulation.Count / villagerCount;
                float richerPopulation = tempVillagers.Count - villagersInThisPopulation.Count;
                //tempVillagers = tempVillagers.Except(villagersToDelete).ToList();
                cumulativeFraction += fractionOfPopulation;
                if (cumulativeFraction >= 1)
                {
                    break;
                }

                scores[i] = percentiles[i] * (fractionOfPopulation + (2 * (richerPopulation)));
            }
        }
        return scores;
    }
}
