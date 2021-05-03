using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationBehaviourCompetition : MonoBehaviour
{
    private int generation;
    private FloorZoneCompetition floorzone;
    private FruitBushControllerAdvanced controller;
    private VillagerStatsEvolution villagerStats;
    public List<string> evolutionValues;
    private int fruitCountOfGeneration = 0;
    private bool initialised;
    public float timer;
    // Start is called before the first frame update
    private void Start()
    {
        evolutionValues = new List<string>();
        generation = 0;
        initialised = false;
        timer = 0;
    }

    private void NewGeneration()
    {
      //  RecordCurrentGenerationData();
        floorzone.Reproduce(generation + 1);
        floorzone.ResetFruitCountInGeneration();
        controller.ReplenishBushes();
        controller.ProduceNewBushes(Random.Range(3, 7));
        generation += 1;
    }



    public int GetCurrentGeneration()
    {
        return generation + 1;
    }

    private void RecordCurrentGenerationData()
    {
        //juu
        //sb.Append(villagerStats.GetGeneration()).Append(",").Append(floorzone.GetFruitCount()).Append(",");
        //sb.Append(floorzone.GetFruitCountInGeneration()).Append(",").Append(villagerStats.GetAverageDesireToReveal()).Append(",").Append(villagerStats.GetAverageDesireToShare());
        //sb.Append(",").Append(villagerStats.GetAverageEnergyLossRate()).Append(",").Append(villagerStats.GetAverageEnergyValue()).Append(",").Append(villagerStats.GetAverageSpeed());
        //sb.Append(",").Append(villagerStats.GetAverageCarryingCapacity()).Append(",").Append(floorzone.GetFittestCandidate());
        //evolutionValues.Add(sb.ToString());
    }

    private void FixedUpdate()
    {
        if (!initialised)
        {
            floorzone = FindObjectOfType<FloorZoneCompetition>();
            controller = FindObjectOfType<FruitBushControllerAdvanced>();
           // villagerStats = FindObjectOfType<VillagerStatsEvolution>();
            floorzone.InitialSpawning();
            initialised = true;
        }
        if (timer >= 30)
        {
            if (GameObject.FindGameObjectsWithTag("Villager").Length <= 0)
            {
                Debug.Log("Can't reproduce");
            }
            else
            {
                NewGeneration();
                timer = 0;
            }

        }
        timer += Time.deltaTime;
    }


}

