using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VillagePopulation : MonoBehaviour
{
    private TMP_Text population;
    private GetVillagerStats stats;
    bool statsExists = false;
    void Update()
    {
        if (!statsExists)
        {
            stats = FindObjectOfType<GetVillagerStats>();
            population = GameObject.Find("Population").GetComponent<TMP_Text>();
        }
        population.text = "Population: " + stats.VillagersAlive();
    }
}
