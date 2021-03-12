using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class VillagerStats : MonoBehaviour
{
    private TMP_Text averageFruitCollected;
    private TMP_Text leastFruitCollected;
    private TMP_Text mostFruitCollected;
    private int averageCount = 0;
    private int leastCount = 0;
    private int mostCount = 0;
    int least = int.MaxValue;
    int most = int.MinValue;

    // Start is called before the first frame update
    void Start()
    {
         averageFruitCollected = GameObject.Find("AverageCollected").GetComponent<TMP_Text>();
         leastFruitCollected = GameObject.Find("MostCollected").GetComponent<TMP_Text>();
         mostFruitCollected = GameObject.Find("LeastCollected").GetComponent<TMP_Text>();

        if (averageFruitCollected != null && leastFruitCollected != null && mostFruitCollected != null)
        {
            averageFruitCollected.text = $"Average collected fruit (per villager): 0";
            leastFruitCollected.text = $"Villager least collected fruit : 0";
            mostFruitCollected.text = $"Villager most collected fruit : 0";

        }
    }

    // Update is called once per frame
    void Update()
    {

            averageFruitCollected.text = $"Average collected fruit (per villager): {GetAverageFruit()}";

            leastFruitCollected.text = $"Villager least collected fruit : {GetLeastFruit()}";

            mostFruitCollected.text = $"Villager most collected fruit : {GetMostFruit()}";

    }

    int GetAverageFruit()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        if (villagers == null)
        {
            return 0;
        }

        return villagers.Sum(v => v.GetComponent<AgentVillager1>()?.getFruitCollected() ?? 0) / villagers.Length;
    }

    int GetLeastFruit()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");

        foreach (GameObject villager in villagers)
        {
            int locallLeast = villager.GetComponent<AgentVillager1>().getFruitCollected();
            if (locallLeast < least)
            {
                least = locallLeast;
            }
        }
        return least;  
    }

    int GetMostFruit()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");

        foreach (GameObject villager in villagers)
        {
            int localMost = villager.GetComponent<AgentVillager1>().getFruitCollected();
            if (localMost > most)
            {
                most = localMost;
            }
        }
        return most;
    }
}
