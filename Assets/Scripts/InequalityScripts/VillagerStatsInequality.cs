using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VillagerStatsInequality : VillagerStats
{
    private TMP_Text dominantP;
    private TMP_Text dominantStr;
    private TMP_Text dominantSpd;
    private FloorZoneAdvanced floorZone;
    private bool foundFloorZone;
    private int dominantPCount = int.MinValue;
    void Start()
    {
        dominantP = GameObject.Find("DominantPersonality").GetComponent<TMP_Text>();
    }

    public KeyValuePair<AgentVillagerAdvanced.Personality, int> getDominantPersonality()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        int pCount = 0;
        foreach  (GameObject villager in villagers)
        {
            if (villager.GetComponent<AgentVillagerAdvanced>().personality == floorZone.GetDominantPersonality())
            {
                pCount++;
            }
        }
        KeyValuePair< AgentVillagerAdvanced.Personality, int> dominantP = new KeyValuePair<AgentVillagerAdvanced.Personality, int>(floorZone.GetDominantPersonality(), pCount);
        dominantPCount = pCount;
        return dominantP;
    }

    void Update()
    {
        if (!foundFloorZone)
        {
            floorZone = FindObjectOfType<FloorZoneAdvanced>();
        }
        if (getDominantPersonality().Value > dominantPCount)
        {
           // dominantP.text =  FloorZone  getDominantPersonality().ToString();
        }
        else
        {
            dominantP.text = "No dominant Personality";
        }
    }
}
