using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationBehavioursEvolution : MonoBehaviour
{
    private int generation;
    private FloorZoneEvolution floorzone;
    private FruitBushControllerAdvanced controller;
    private GetVillagerStats villagerStats;
    private int fruitCountOfGeneration = 0;
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

    private void FixedUpdate()
    {
        if (!initialised)
        {
            floorzone = FindObjectOfType<FloorZoneEvolution>();
            controller = FindObjectOfType<FruitBushControllerAdvanced>();      
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

