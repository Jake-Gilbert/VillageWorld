using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GiniCalculatorInequality : GiniCalculator
{
    public List<string> inequalityValues;
    public FloorZoneAdvanced floorZone;
    [SerializeField]
    private VillagerStatsInequality villagerStatsInequality;
    [SerializeField]
    private GenerationBehaviours behaviours;
    private int villagerCount = 0;
    private int currentGeneration = 1;
    public bool running = true;
    private bool savedInfoForGeneration = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    private IEnumerator MakeTrue()
    {
        yield return new WaitForSeconds(2);
        savedInfoForGeneration = false;
    }

    private void Update()
    {
        if (floorZone == null)
        {
            floorZone = FindObjectOfType<FloorZoneAdvanced>();
        }
        if (currentGeneration >= 21 || GameObject.FindGameObjectsWithTag("Villager").Length <= 0)
        {
            running = false;
        }
        if (savedInfoForGeneration) 
        {
            StartCoroutine(MakeTrue());
        }
        if (behaviours.timer >= 29.5F && running && !savedInfoForGeneration)
        {
            Debug.Log(string.Join(",",GetVillagerScores()));
            StringBuilder sb = new StringBuilder();
            sb.Append(currentGeneration);
            sb.Append(",");
            sb.Append(villagerStatsInequality.GetMaximumAliveInGeneration(currentGeneration));
            sb.Append(",");
            sb.Append(GameObject.FindGameObjectsWithTag("Villager").Length);
            sb.Append(",");
            sb.Append(FindObjectOfType<FloorZoneAdvanced>().GetFruitCount());
            sb.Append(",");
            sb.Append(FindObjectOfType<FloorZoneAdvanced>().GetFruitCountInGeneration());
            sb.Append(",");
            sb.Append(GameObject.FindGameObjectsWithTag("Fruit").Length);
            sb.Append(",");
            sb.Append(getVillagerStats.GetLeastFruit().ToString());
            sb.Append(",");
            sb.Append(getVillagerStats.GetMostFruit().ToString());
            sb.Append(",");
            sb.Append(getVillagerStats.GetAverageFruit().ToString());
            sb.Append(",");
            SortedList<AgentVillagerAdvanced.Personality, int> personalitiesTemp = villagerStatsInequality.GetPersonalitiesAndQuantities();
            sb.Append(personalitiesTemp.Keys.ToList()[personalitiesTemp.IndexOfValue(personalitiesTemp.Values.Max())]);
            sb.Append(",");
            SortedList<AgentVillagerAdvanced.SpeedTrait, int> strengthsTemp = villagerStatsInequality.GetSpeedsAndQuantities();
            sb.Append(strengthsTemp.Keys.ToList()[strengthsTemp.IndexOfValue(strengthsTemp.Values.Max())]);
            sb.Append(",");
            SortedList<AgentVillagerAdvanced.StrengthTrait, int> speedsTemp = villagerStatsInequality.GetStrengthsAndQuantities();
            sb.Append(speedsTemp.Keys.ToList()[strengthsTemp.IndexOfValue(strengthsTemp.Values.Max())]);
            sb.Append(",");
            sb.Append(villagerStatsInequality.GetQuantityOfPersonality(AgentVillagerAdvanced.Personality.Selfish));
            sb.Append(",");
            sb.Append(villagerStatsInequality.GetQuantityOfPersonality(AgentVillagerAdvanced.Personality.Neutral));
            sb.Append(",");
            sb.Append(villagerStatsInequality.GetQuantityOfPersonality(AgentVillagerAdvanced.Personality.Empathetic));
            sb.Append(",");
            float[] scores = GetVillagerScores();
            float gini = CalculateGiniCoefficient(scores);
            sb.Append(gini);
            inequalityValues.Add(sb.ToString());
            savedInfoForGeneration = true;
            currentGeneration += 1;
        }  

    }

    public new float[] GetVillagerScores()
    {
        int fruitCollected = floorZone.GetFruitCountInGeneration();
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        if (villagerCount == 0)
        {
            villagerCount = GameObject.FindGameObjectsWithTag("Villager").Length;
        }

        float[] percentiles = new float[] { 0.02F, 0.15F, 0.3F, 0.5F };
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
            Debug.Log("Proportion " + string.Join(",", proportionOfWealth));
            for (int i = 0; i < scores.Length; i++)
            {
                float richer = CalculateFractionOfRicher(thresholds[i], wealthPerCitizen);
                float percentageOfPopulation = populationDivide[i] / villagerCount;
                scores[i] = percentageOfWealth[i] * (percentageOfPopulation + (2 * richer));
                scores[i] = (float)System.Math.Round(scores[i], 3);
            }
        }
        return scores;
    }
}
