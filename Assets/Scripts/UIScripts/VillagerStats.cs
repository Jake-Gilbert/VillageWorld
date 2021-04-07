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
    private GetVillagerStats getVillagerStats;
    private int averageCount = 0;
    private int leastCount = 0;
    private int mostCount = 0;
    int least = int.MaxValue;
    int most = int.MinValue;

    // Start is called before the first frame update
    void Start()
    {
        averageFruitCollected = GameObject.Find("AverageCollected").GetComponent<TMP_Text>();
        leastFruitCollected = GameObject.Find("LeastCollected").GetComponent<TMP_Text>();
        mostFruitCollected = GameObject.Find("MostCollected").GetComponent<TMP_Text>();
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
        if (getVillagerStats == null)
        {
            getVillagerStats = FindObjectOfType<GetVillagerStats>();
        }

        averageFruitCollected.text = $"Average collected fruit (per villager): {getVillagerStats.GetAverageFruit()}";

        leastFruitCollected.text = $"Villager least collected fruit : {getVillagerStats.GetLeastFruit()}";

        mostFruitCollected.text = $"Villager most collected fruit : {getVillagerStats.GetMostFruit()}";
    }
}