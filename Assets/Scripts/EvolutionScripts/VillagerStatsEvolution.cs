using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class VillagerStatsEvolution : VillagerStats
{
    public TMP_Text generation;
    public TMP_Text population;
    public TMP_Text averageTraits;
    public TMP_Text averageTraits2;
    public TMP_Text fruitCollected;
    public SortedList<int, int> maximumAliveInGeneration = new SortedList<int, int>();
    private int maximumAlive = 0;
    private FloorZoneEvolution floorZone;
    private bool foundFloorZone;
    private int currentGeneration = 1;
    private float timer = 0;
    void Start()
    {

    }

    public int GetGeneration()
    {
        return FindObjectOfType<GenerationBehavioursEvolution>().GetCurrentGeneration();
    }

    public int GetMaximumAliveInGeneration(int generation)
    {
        return maximumAliveInGeneration[generation];
    }
    private void Update()
    {
        generation.text = $"Generation: {GetGeneration()}";
        population.text = $"Population: {GameObject.FindGameObjectsWithTag("Villager").Length}";
        averageTraits.text = $"Trait averages: \n\n" +
            $"Average energy maximum: {GetAverageEnergyValue()} \n\n" +
            $"Average carrying capacity: {GetAverageCarryingCapacity()} \n\n" +
            $"Average speed: {GetAverageSpeed()}";
        averageTraits2.text = $"Traits averages: \n\n " +
            $"Average desire to share: { GetAverageDesireToShare()} \n\n" +
            $"Average desire to reveal: {GetAverageDesireToReveal()}";
        fruitCollected.text = $"Fruit stats: " +
            $"Total fruit collected: {floorZone.GetFruitCount()} \n\n" +
            $"Fruit count collected in genreation: {floorZone.GetFruitCountInGeneration()}";

}

    public int GetAverageFruit()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        if (villagers.Length <= 0)
        {
            return 0;
        }
        return villagers.Sum(v => v.GetComponent<AgentVillagerEvolution>()?.GetFruitCollected() ?? 0) / villagers.Length;
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
            int localLeast = villager.GetComponent<AgentVillagerEvolution>().GetFruitCollected();
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
            int localMost = villager.GetComponent<AgentVillagerEvolution>().GetFruitCollected();
            if (localMost > most)
            {
                most = localMost;
            }
        }
        return most;
    }

    public float GetAverageEnergyValue()
    {
        return GameObject.FindGameObjectsWithTag("Villager").Average(x => x.GetComponent<AgentVillagerEvolution>().deadEnergy.sizeDelta.x);
    }
    public double GetAverageCarryingCapacity()
    {
        return System.Math.Round(GameObject.FindGameObjectsWithTag("Villager").Average(x => x.GetComponent<AgentVillagerEvolution>().carryingCapacity), 3);
    }

    public double GetAverageSpeed()
    {
        return System.Math.Round(GameObject.FindGameObjectsWithTag("Villager").Average(x => x.GetComponent<AgentVillagerEvolution>().speed), 3);
    }

    public double GetAverageDesireToShare()
    {
        return System.Math.Round(GameObject.FindGameObjectsWithTag("Villager").Average(x => x.GetComponent<AgentVillagerEvolution>().desireToShare), 3);
    }

    public double GetAverageDesireToReveal()
    {
        return System.Math.Round(GameObject.FindGameObjectsWithTag("Villager").Average(x => x.GetComponent<AgentVillagerEvolution>().desireToReveal), 3);
    }
    void FixedUpdate()
    {
        if (currentGeneration == FindObjectOfType<GenerationBehavioursEvolution>().GetCurrentGeneration())
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
        if (currentGeneration < FindObjectOfType<GenerationBehavioursEvolution>().GetCurrentGeneration())
        {
            currentGeneration = FindObjectOfType<GenerationBehavioursEvolution>().GetCurrentGeneration();
            maximumAlive = 0;
        }
        else if (maximumAlive < GameObject.FindGameObjectsWithTag("Villager").Length)
        {
            maximumAlive = GameObject.FindGameObjectsWithTag("Villager").Length;
        }
        timer += Time.deltaTime;
        if (!foundFloorZone)
        {
            floorZone = FindObjectOfType<FloorZoneEvolution>();
        }
    }
}