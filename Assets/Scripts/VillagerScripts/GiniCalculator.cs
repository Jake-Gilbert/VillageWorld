using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;



public class GiniCalculator : MonoBehaviour
{
    public TMP_Text fruitCollectedText;
    public FloorZone floorZone;
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
            float gini = CalculateGiniCoefficient();
            Debug.Log(gini);
            giniValues.Add(gini);
            timer = 0;
        }
    }

    float CalculateGiniCoefficient()
    {
        float[] scores = getVillagerScores();
        Debug.Log("scores " + string.Join(",", scores));
        float giniValue = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            giniValue += scores[i];
        }
        return 1 - giniValue;
    }

    float[] CalculateThresholds(float[] percentiles)
    {
        float[] thresholds = new float[percentiles.Length];
        int i = 0;
        GameObject floorzone = GameObject.FindGameObjectWithTag("Zone");
        foreach (int percentile in percentiles)
        {

            thresholds[i] = (int)System.Math.Round(percentiles[i] * floorzone.GetComponent<FloorZone>().GetFruitCount(), 1);
            i += 1;
        }
        return thresholds;
    }
    float[] getVillagerScores()
    {
        floorZone = GameObject.FindGameObjectWithTag("Zone").GetComponent<FloorZone>();
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        if (villagerCount == 0)
        {
            villagerCount = GameObject.FindGameObjectsWithTag("Villager").Length;
        }

        float[] percentiles = new float[] { 0.1F, 0.2F, 0.3F, 0.4F };
        float[] thresholds = CalculateThresholds(percentiles);
        int[] proportionOfWealth = new int[4];
        int[] wealthPerCitizen = WealthPerVillager();
        Debug.Log("PER CITIZEN " + string.Join(",", wealthPerCitizen));
        float[] scores = new float[4];
        int[] populationDivide = CalculatePopulationDivide(thresholds, wealthPerCitizen);
        if (floorZone.GetFruitCount() == 0)
        {
            proportionOfWealth[0] = 1;
            float percentageOfPopulation = populationDivide[0] / villagers.Length;
            scores[0] = proportionOfWealth[0] * (percentageOfPopulation + 2 * 0);         
        }
        else
        {
            proportionOfWealth = CalculateWealthDivide(thresholds, wealthPerCitizen);
            Debug.Log("Proportion " + string.Join(",", proportionOfWealth));
            for (int i = 0; i < scores.Length; i++)
            {
                float richer = CalculateFractionOfRicher(thresholds[i], wealthPerCitizen);
                float percentageOfPopulation =  populationDivide[i] / villagerCount;
                float percentageOfWealth = (float)System.Math.Round((float)proportionOfWealth[i] / (float)floorZone.GetFruitCount(), 2); ;
                scores[i] = percentageOfWealth * (percentageOfPopulation + (2 * richer));
                Debug.Log(percentageOfWealth + " " + percentageOfPopulation + " " + richer);
            }
        }
        //Debug.Log("pop " + string.Join(",", populationDivide));
        Debug.Log("wealth " + string.Join(",", proportionOfWealth));


        return scores;
    }

    //FOR TESTING
    public int[] CalculatePopulationDivide(float[] thresholds, int[] villagers)
    {
        int[] populationDivide = new int[4];
        List<int> temporaryVillagers = villagers.ToList();
        List<int> checkedNumbers = new List<int>();
        for (int i = 0; i < thresholds.Length; i++)
        {
            populationDivide[i] = 0;
            if (villagers.Length > 0)
            {
                foreach (int villager in temporaryVillagers)
                {
                    if (villager <= thresholds[i])
                    {
                        if (!checkedNumbers.Contains(villager))
                        {
                            populationDivide[i] += temporaryVillagers.Count(x => x == thresholds[i]);
                            checkedNumbers.Add(villager);
                        }
                    }
                }
            }
            else
            {
                break;
            }
        }
        return populationDivide;
    }

    int[] CalculateWealthDivide(float[] thresholds, int[] wealthPerVillager)
    {
        int[] wealthPerGroup = new int[4];
        List<int> excludedVillagers = new List<int>();
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (wealthPerVillager.Length > 0)
            {
                foreach (int villager in wealthPerVillager)
                {
                    if (villager <= thresholds[i])
                    {
                        if (!excludedVillagers.Contains(villager))
                        {
                            wealthPerGroup[i] += wealthPerVillager.Count(x => x == thresholds[i]);
                            excludedVillagers.Add(villager);
                        }
                    }
              
                }
            }
        }
        return wealthPerGroup;
    }

float CalculateFractionOfRicher(float percentile, int[] villagers)
{
    float fraction = 0;
    foreach (int villager in villagers)
    {
        if (villager > percentile)
        {
            fraction += 1;
        }
    }
    return fraction / villagers.Length;
}

int[] WealthPerVillager()
{
    GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
    int[] fruitPerCitizen = new int[villagers.Length];
    for (int i = 0; i < fruitPerCitizen.Length; i++)
    {
        fruitPerCitizen[i] = villagers[i].GetComponent<AgentVillager1>().getFruitCollected();
    }
    System.Array.Sort(fruitPerCitizen);
    return fruitPerCitizen;
}



    //int[] CalculatePercentiles(int amount, int num)
    //{
    //    List<int> selectedIndexes = new List<int>();
    //    for (int i = 0; i < amount - 1; i++)
    //    {
    //        //Random randomIndex = new System.Random();
    //        selectedIndexes.Add(Random.Range(0, num + 1));
    //    }

    //    selectedIndexes = selectedIndexes.OrderByDescending(x => x).ToList();

    //    List<int> result = new List<int>();
    //    int leftOperand = num;
    //    for (int i = 0; i < selectedIndexes.Count(); i++)
    //    {
    //        result.Add(leftOperand - selectedIndexes[i]);
    //        leftOperand = selectedIndexes[i];
    //    }

    //    result.Add(leftOperand);

    //    return result.OrderBy(x => x).ToArray();
    //}

}


