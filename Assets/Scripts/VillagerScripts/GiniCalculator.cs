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
            Debug.Log(CalculateGiniCoefficient(GetVillagerScores()));
            firstRun = true;
        }
        timer += Time.deltaTime;

        if (timer >= 5)
        {
            float[] scores = GetVillagerScores();
            Debug.Log("scores " + string.Join(",", scores));
            float gini = CalculateGiniCoefficient(scores);
            Debug.Log(gini);
            giniValues.Add(gini);
            timer = 0;
        }
    }

    public float CalculateGiniCoefficient(float[] scores)
    {
        float giniValue = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            giniValue += scores[i];
        }
        return 1 - giniValue;
    }

    public float[] CalculateThresholds(float[] percentiles, int fruitCount)
    {
        float[] thresholds = new float[percentiles.Length];
        int i = 0;
        foreach (int percentile in percentiles)
        {
            thresholds[i] = (float)System.Math.Round(percentiles[i] * fruitCount);
            i += 1;
        }
        return thresholds;
    }
    float[] GetVillagerScores()
    {
        int fruitCollected = FruitCollected();
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        if (villagerCount == 0)
        {
            villagerCount = GameObject.FindGameObjectsWithTag("Villager").Length;
        }

        float[] percentiles = new float[] { 0.1F, 0.2F, 0.3F, 0.4F };
        float[] thresholds = CalculateThresholds(percentiles, fruitCollected);
        Debug.Log("Threshholds " + string.Join(",", thresholds));
        int[] proportionOfWealth = new int[4];
        int[] wealthPerCitizen = WealthPerVillager();
        Debug.Log("PER CITIZEN " + string.Join(",", wealthPerCitizen));
        float[] scores = new float[4];
        int[] populationDivide = CalculatePopulationDivide(thresholds, wealthPerCitizen);
        if (fruitCollected == 0)
        {
            proportionOfWealth[0] = 1;
            float percentageOfPopulation = populationDivide[0] / villagers.Length;
            scores[0] = proportionOfWealth[0] * (percentageOfPopulation + 2 * 0);
        }
        else
        {
            proportionOfWealth = CalculateQuantityOfWealth(thresholds, wealthPerCitizen);
            float[] percentageOfWealth = CalculateProportionOfWealth(proportionOfWealth, fruitCollected);
            Debug.Log("Proportion " + string.Join(",", proportionOfWealth));
            for (int i = 0; i < scores.Length; i++)
            {
                float richer = CalculateFractionOfRicher(thresholds[i], wealthPerCitizen);
                float percentageOfPopulation = populationDivide[i] / villagerCount;
                scores[i] = (float)percentageOfWealth[i] * (percentageOfPopulation + (2 * richer));
                scores[i] = (float) System.Math.Round( scores[i], 3);
            }
        }


        return scores;
    }

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

    public float[] CalculateProportionOfWealth(int[] proportionOfWealth, int fruitCollected)
    {
        float[] wealthProportion = new float[4];
        for (int i = 0; i < proportionOfWealth.Length; i++)
        {
            wealthProportion[i] = (float)proportionOfWealth[i] / fruitCollected;
            wealthProportion[i] = (float) System.Math.Round(wealthProportion[i], 3);
        }
        Debug.Log("fruit collected " + fruitCollected);
        Debug.Log("proportion of welath " + string.Join(",", wealthProportion));
        return wealthProportion;
    }
    public int[] CalculateQuantityOfWealth(float[] thresholds, int[] wealthPerVillager)
    {
        int[] wealthPerGroup = new int[4];
        Dictionary<int, int> villagersAndWealth = new Dictionary<int, int>();
        for (int i = 0; i < wealthPerVillager.Length; i++)
        {
            villagersAndWealth.Add(i, wealthPerVillager[i]);
        }
        for (int i = 0; i < thresholds.Length; i++)
        {
            for (int j = 0;  j < villagersAndWealth.Count; j++)
            {
                if (villagersAndWealth[j] <= thresholds[i])
                {
                    wealthPerGroup[i] += villagersAndWealth[j];
                    villagersAndWealth[j] = 0;
                }
            }
        }
        Debug.Log("wealth per group: " + string.Join(",", wealthPerGroup));
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
    int FruitCollected()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        int fruitCollected = 0;
        for (int i = 0; i < villagers.Length; i++)
        {
            fruitCollected += villagers[i].GetComponent<AgentVillager1>().getFruitCollected();
        }
        return fruitCollected;
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


