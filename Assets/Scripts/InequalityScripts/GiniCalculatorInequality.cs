using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GiniCalculatorInequality : GiniCalculator
{
    public List<string> inequalityValues;
    private VillagerStatsInequality villagerStatsInequality;
    private int currentGeneration = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (currentGeneration != FindObjectOfType<GenerationBehaviours>().GetCurrentGeneration())
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(currentGeneration);
            sb.Append(",");
            sb.Append(GameObject.FindGameObjectsWithTag("Villager").Length);
            currentGeneration = FindObjectOfType<GenerationBehaviours>().GetCurrentGeneration();

        }
        //sb.Append(villagerStatsInequality);
        //float[] scores = GetVillagerScores();
        //float gini = CalculateGiniCoefficient(scores);
        //StringBuilder sb = new StringBuilder();
        //sb.Append(getVillagerStats.VillagersAlive().ToString());
        //sb.Append(",");
        //sb.Append(getVillagerStats.GetLeastFruit().ToString());
        //sb.Append(",");
        //sb.Append(getVillagerStats.GetMostFruit().ToString());
        //sb.Append(",");
        //sb.Append(getVillagerStats.GetAverageFruit().ToString());
        //sb.Append(",");
        //sb.Append(getVillagerStats.GetTotalFruitCollected());
        //sb.Append(",");
        //sb.Append(gini.ToString());
        //giniValues.Add(sb.ToString());
        //timer = 0;
    }
    }

}
