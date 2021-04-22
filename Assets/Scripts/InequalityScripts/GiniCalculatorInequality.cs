using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GiniCalculatorInequality : GiniCalculator
{
    public List<string> inequalityValues;
    [SerializeField]
    private VillagerStatsInequality villagerStatsInequality;
    [SerializeField]
    private GenerationBehaviours behaviours;
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
        if (currentGeneration > 10)
        {
            running = false;
        }
        if (savedInfoForGeneration) 
        {
            StartCoroutine(MakeTrue());
        }
        if (behaviours.timer >= 29.5F && running && !savedInfoForGeneration)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(currentGeneration);
            sb.Append(",");
            sb.Append(villagerStatsInequality.GetMaximumAliveInGeneration(currentGeneration));
            sb.Append(",");
            sb.Append(GameObject.FindGameObjectsWithTag("Villager").Length);
            sb.Append(",");
            sb.Append(FindObjectOfType<FloorZoneAdvanced>().GetFruitCount());
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
}
