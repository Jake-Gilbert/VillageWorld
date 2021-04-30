using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;



public class GiniCalculator : MonoBehaviour
{
    public GetVillagerStats getVillagerStats;
    public List<string> giniValues;
    private int villagerCount = 0;
    private float timer = 0;
    private bool firstRun = false;
    private bool valid = true;
    private void Start()
    {
        giniValues = new List<string>();
    }

    private void Update()
    {
        if (!firstRun)
        {
            getVillagerStats = FindObjectOfType<GetVillagerStats>();
            Debug.Log(CalculateGiniCoefficient(GetVillagerScores()));
            firstRun = true;
        }
        timer += Time.deltaTime;
        if (!VillagersExist() || !BushesExist())
        {
            valid = false;            
        }
        else
        {
            if (timer >= 5 && valid)
            {
                float[] scores = GetVillagerScores();
                float gini = CalculateGiniCoefficient(scores);
                StringBuilder sb = new StringBuilder();
                sb.Append(getVillagerStats.VillagersAlive().ToString());
                sb.Append(",");
                sb.Append(getVillagerStats.GetLeastFruit().ToString());
                sb.Append(",");
                sb.Append(getVillagerStats.GetMostFruit().ToString());
                sb.Append(",");
                sb.Append(getVillagerStats.GetAverageFruit().ToString());
                sb.Append(",");
                sb.Append(getVillagerStats.GetTotalFruitCollected());
                sb.Append(",");
                sb.Append(gini.ToString());
                giniValues.Add(sb.ToString());
                timer = 0;
            }
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

    protected bool VillagersExist()
    {
        return GameObject.FindGameObjectsWithTag("Villager").Length > 0 ? true : false;
    }

    protected bool BushesExist()
    {
        return GameObject.FindGameObjectsWithTag("Bush").Length > 0 ? true : false;
    }

    public float[] GetVillagerScores()
    {
        int fruitCollected = getVillagerStats.GetTotalFruitCollected();
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        if (villagerCount == 0)
        {
            villagerCount = GameObject.FindGameObjectsWithTag("Villager").Length;
        }

        float[] percentiles = new float[] { 0.1F, 0.2F, 0.3F, 0.4F };
        float[] thresholds = CalculateThresholds(percentiles, fruitCollected);
        int[] proportionOfWealth = new int[4];
        int[] wealthPerCitizen = WealthPerVillager();
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
            for (int i = 0; i < scores.Length; i++)
            {
                float richer = CalculateFractionOfRicher(thresholds[i], wealthPerCitizen);
                float percentageOfPopulation = populationDivide[i] / villagerCount;
                scores[i] = percentageOfWealth[i] * (percentageOfPopulation + (2 * richer));
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
        return wealthPerGroup;
    }

    protected float CalculateFractionOfRicher(float percentile, int[] villagers)
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
    protected int FruitCollected()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        int fruitCollected = 0;
        for (int i = 0; i < villagers.Length; i++)
        {
            fruitCollected += villagers[i].GetComponent<AgentVillagerAdvanced>().GetFruitCollected();
        }
        return fruitCollected;
    }
    protected int[] WealthPerVillager()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        int[] fruitPerCitizen = new int[villagers.Length];
        for (int i = 0; i < fruitPerCitizen.Length; i++)
        {
            fruitPerCitizen[i] = villagers[i].GetComponent<AgentVillagerAdvanced>().GetFruitCollected();
        }
        System.Array.Sort(fruitPerCitizen);
        return fruitPerCitizen;
    }
}


