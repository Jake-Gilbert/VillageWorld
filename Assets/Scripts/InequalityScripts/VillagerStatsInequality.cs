using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VillagerStatsInequality : VillagerStats
{
    private TMP_Text dominantP;
    private TMP_Text dominantStr;
    private SortedList<int, int> maximumAliveInGeneration = new SortedList<int, int>();
    private int maximumAlive = 0;
    private TMP_Text dominantSpd;
    private FloorZoneAdvanced floorZone;
    private bool foundFloorZone;
    private int currentGeneration = 1;
    private int dominantPCount = int.MinValue;
    private float timer = 0;
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
        //dominantPCount = pCount;
        return dominantP;
    }
    
    public SortedList<AgentVillagerAdvanced.Personality, int> GetPersonalitiesAndQuantities()
    {
        return floorZone.GetPersonalitiesAndQuantities();
    }

    public SortedList<AgentVillagerAdvanced.StrengthTrait, int> GetStrengthsAndQuantities()
    {
        return floorZone.getStrengthsAndQuantities();
    }

    public SortedList<AgentVillagerAdvanced.SpeedTrait, int> GetSpeedsAndQuantities()
    {
        return floorZone.GetSpeedsAndQuantities();
    }

    public int GetGeneration()
    {
        return FindObjectOfType<GenerationBehaviours>().GetCurrentGeneration();
    }

    public int GetMaximumAliveInGeneration(int generation)
    {
        return maximumAliveInGeneration[generation];
    }
    private void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (currentGeneration == FindObjectOfType<GenerationBehaviours>().GetCurrentGeneration())
        {
            if (!maximumAliveInGeneration.Keys.Contains(currentGeneration))
            {
                maximumAliveInGeneration.Add(currentGeneration, maximumAlive);
            }
            else if (maximumAlive > maximumAliveInGeneration[currentGeneration])
            {
                maximumAliveInGeneration[currentGeneration] = maximumAlive;
            }
        }
        if (currentGeneration < FindObjectOfType<GenerationBehaviours>().GetCurrentGeneration())
        {
            currentGeneration = FindObjectOfType<GenerationBehaviours>().GetCurrentGeneration();
            maximumAlive = 0;
        }
        else if(maximumAlive < GameObject.FindGameObjectsWithTag("Villager").Length)
        {
            maximumAlive = GameObject.FindGameObjectsWithTag("Villager").Length;
        }
        //else if (maximumAliveInGeneration < GameObject.FindGameObjectsWithTag("Villager").Length)
        //{
        //    maximumAliveInGeneration = GameObject.FindGameObjectsWithTag("Villager").Length;
        //}
        if (timer > 2)
        {
            if (getDominantPersonality().Value > dominantPCount)
            {
                dominantP.text = floorZone.GetDominantPersonality().ToString();
            }
            timer = 0;
        }
        timer += Time.deltaTime;
        if (!foundFloorZone)
        {
            floorZone = FindObjectOfType<FloorZoneAdvanced>();
        }
    }
}
