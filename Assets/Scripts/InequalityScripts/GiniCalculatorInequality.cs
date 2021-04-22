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
    private GetVillagerStats villagerStats;
    private int currentGeneration = 1;
    public bool running = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (currentGeneration > 10)
        {
            running = false;
        }
        if (currentGeneration != FindObjectOfType<GenerationBehaviours>().GetCurrentGeneration() && running)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(currentGeneration);
            sb.Append(",");
            sb.Append(villagerStatsInequality.GetMaximumAliveInGeneration(currentGeneration));
            sb.Append(",");
            sb.Append(villagerStats.VillagersAlive());
            sb.Append(",");
            sb.Append(villagerStats.GetTotalFruitCollected());
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
            sb.Append(string.Join(",", personalitiesTemp));
            sb.Append(",");
            sb.Append(string.Join(",", speedsTemp));
            sb.Append(",");
            sb.Append(string.Join(",", strengthsTemp));
            float[] scores = GetVillagerScores();
            sb.Append(",");
            float gini = CalculateGiniCoefficient(scores);
            sb.Append(gini);
            inequalityValues.Add(sb.ToString());
            currentGeneration = FindObjectOfType<GenerationBehaviours>().GetCurrentGeneration();
        }

    }
}
