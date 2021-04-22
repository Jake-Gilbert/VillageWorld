using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationBehaviours : MonoBehaviour
{
    private int generation;
    private FloorZoneAdvanced floorzone;
    private FruitBushControllerAdvanced controller;
    private GetVillagerStats villagerStats;
    private bool initialised;
    public float timer;
    // Start is called before the first frame update
    private void Start()
    {
        generation = 0;
        initialised = false;
        timer = 0;
    }

    private void NewGeneration()
    {
        floorzone.Reproduce();
        controller.ReplenishBushes();
        controller.ProduceNewBushes(Random.Range(3, 7));
        generation += 1;
    }

    public int GetCurrentGeneration()
    {
        return generation + 1;
    }

    private void FixedUpdate()
    {
        if (!initialised) 
        {
            floorzone = FindObjectOfType<FloorZoneAdvanced>();
            controller = FindObjectOfType<FruitBushControllerAdvanced>();
            villagerStats = FindObjectOfType<GetVillagerStats>();
            floorzone.InitialSpawning();
            initialised = true;
        }
        if (timer >= 30 || timer >= 1 && villagerStats.NoFruitBushes())
        {
            NewGeneration();
            timer = 0;
        }
        timer += Time.deltaTime;
    }


}
