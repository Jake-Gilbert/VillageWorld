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
    private float timer;
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
        generation += 1;
    }


    public int GetCurrentGeneration()
    {
        return generation + 1;
    }

    private void Update()
    {
        if (!initialised) 
        {
            floorzone = FindObjectOfType<FloorZoneAdvanced>();
            controller = FindObjectOfType<FruitBushControllerAdvanced>();
            villagerStats = FindObjectOfType<GetVillagerStats>();          
            floorzone.InitialSpawning();
            initialised = true;
        }
        if (timer >= 15 || villagerStats.NoFruitBushes())
        {
            NewGeneration();
            timer = 0;
        }
        timer += Time.deltaTime;
    }


}
