using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class VillagerStatsInequality : VillagerStats
{
    [SerializeField]
    private TMP_Text dominantP;
    [SerializeField]
    private TMP_Text quantityOfPersonalities;
    //private TMP_Text dominantStr;
    public SortedList<int, int> maximumAliveInGeneration = new SortedList<int, int>();
    private int maximumAlive = 0;
    //private TMP_Text dominantSpd;
    private FloorZoneAdvanced floorZone;
    private bool foundFloorZone;
    private int currentGeneration = 1;
    private int dominantPCount = int.MinValue;
    private float timer = 0;
    void Start()
    {
        //dominantP = GameObject.Find("DominantPersonality").GetComponent<TMP_Text>();
    }

    public KeyValuePair<AgentVillagerAdvanced.Personality, int> getDominantPersonality()
    {
        return new KeyValuePair<AgentVillagerAdvanced.Personality, int>(floorZone.GetDominantPersonality(), floorZone.GetPersonalitiesAndQuantities()[floorZone.GetDominantPersonality()]);
    }

    public int GetQuantityOfPersonality(AgentVillagerAdvanced.Personality personality)
    {
        return GameObject.FindGameObjectsWithTag("Villager").Where(x => x.GetComponent<AgentVillagerAdvanced>().personality == personality).Count();
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
        quantityOfPersonalities.text = string.Join(",",floorZone.GetPersonalitiesAndQuantities());
    }

    public int GetAverageFruit()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        if (villagers.Length <= 0)
        {
            return 0;
        }
        return villagers.Sum(v => v.GetComponent<AgentVillagerAdvanced>()?.GetFruitCollected() ?? 0) / villagers.Length;
    }

    public int GetLeastFruit()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        int least = int.MaxValue;
        if (villagers.Length <= 0)
        {
            return 0;
        }
        foreach (GameObject villager in villagers)
        {
            int localLeast = villager.GetComponent<AgentVillagerAdvanced>().GetFruitCollected();
            if (localLeast < least)
            {
                least = localLeast;
            }
        }
        return least;
    }


    public int GetMostFruit()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        int most = int.MinValue;
        if (villagers.Length <= 0)
        {
            return 0;
        }
        foreach (GameObject villager in villagers)
        {
            int localMost = villager.GetComponent<AgentVillagerAdvanced>().GetFruitCollected();
            if (localMost > most)
            {
                most = localMost;
            }
        }
        return most;
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
        else if (maximumAlive < GameObject.FindGameObjectsWithTag("Villager").Length)
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
