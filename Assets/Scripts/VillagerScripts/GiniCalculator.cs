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
    bool firstRun = false;
    void Start()
    {

        giniValues = new List<float>();
    }

    private void Update()
    {
        if (!firstRun)
        {
            Debug.Log(CalculateGiniCoefficient());
            firstRun = true;
        }
        timer += Time.deltaTime;

        if (timer >= 5)
        {
            Debug.Log(CalculateGiniCoefficient());
            giniValues.Add(CalculateGiniCoefficient());
            timer = 0;
        }
    }

    float CalculateGiniCoefficient()
    {
        float[] scores = getVillagerScores();
        //Debug.Log(string.Join(",", scores));
        float giniValue = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            giniValue += scores[i];
        }
        return giniValue;
    }

    float[] CalculateThresholds(float[] percentiles)
    {
        float[] thresholds = new float[percentiles.Length];
        int i = 0;
        GameObject floorzone = GameObject.FindGameObjectWithTag("Zone");
        foreach (int percentile in percentiles)
        {

            thresholds[i] = (int) System.Math.Round(percentiles[i] * floorzone.GetComponent<FloorZone>().GetFruitCount(), 1);
            i += 1;
        }
        return thresholds;
    }
    float[] getVillagerScores()
    {
        List<GameObject> remainingVillagers = GameObject.FindGameObjectsWithTag("Villager").ToList();
        if (villagerCount == 0)
        {
            villagerCount = GameObject.FindGameObjectsWithTag("Villager").Length;
        }

        float[] percentiles = new float[] { 0F, 0.1F, 0.4F, 0.5F };
        float[] thresholds = CalculateThresholds(percentiles);

        float[] scores = new float[4];
        int[] populationDivide = CalculatePopulationDivide(thresholds);
        Debug.Log(string.Join(",", populationDivide));
        //CalculatePercentiles(4);
        //for (int i = 0; i < scores.Length; i++)
        //{
        //    float richer = CalculateFractionOfRicher(percentiles[i], GameObject.FindGameObjectsWithTag("Villager"))
        //   // scores[i] = ();
        //}


        return scores;
    }

    int[] CalculatePopulationDivide(float[] thresholds)
    {
        int[] villagers = new int[4];
        List<GameObject> tempVillagers = GameObject.FindGameObjectsWithTag("Villager").ToList();
        List<GameObject> excludedVillagers = new List<GameObject>();

        //Step 1 iterate through percentiles
        //for each villager that meets the criteria add one
        //[0, 0.1, 0.4, 0.5]      
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (tempVillagers.Count > 0)
            {
                foreach (GameObject villager in tempVillagers)
                {
                    if (villager.GetComponent<AgentVillager1>().getFruitCollected() <= thresholds[i])
                    {
                        villagers[i] += 1;
                        excludedVillagers.Add(villager);
                    }
                }
                tempVillagers = tempVillagers.Except(excludedVillagers).ToList();
            }           
            else
            {
                break;
            }
        }
        return villagers;
    }

    float CalculateFractionOfRicher(float percentile, GameObject[] villagers)
    {
        float fraction = 0;
        foreach (GameObject villager in villagers)
        {
            if (villager.GetComponent<AgentVillager1>().getFruitCollected() > percentile)
            {
                fraction += 1;
            }
        }
        //Debug.Log(fraction / villagerCount);
        return fraction / villagerCount;
    }


    int[] CalculatePercentiles(int amount, int num)
    {
        List<int> selectedIndexes = new List<int>();
        for (int i = 0; i < amount - 1; i++)
        {
            //Random randomIndex = new System.Random();
            selectedIndexes.Add(Random.Range(0, num + 1));
        }

        selectedIndexes = selectedIndexes.OrderByDescending(x => x).ToList();

        List<int> result = new List<int>();
        int leftOperand = num;
        for (int i = 0; i < selectedIndexes.Count(); i++)
        {
            result.Add(leftOperand - selectedIndexes[i]);
            leftOperand = selectedIndexes[i];
        }

        result.Add(leftOperand);

        return result.OrderBy(x => x).ToArray();
    }

}


