using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetVillagerStats : MonoBehaviour
{
    // Start is called before the first frame update

    public int VillagersAlive()
    {
        return GameObject.FindGameObjectsWithTag("Villager").Length;
    }

    public int GetTotalFruitCollected()
    {
        return FindObjectOfType<FloorZone>().GetFruitCount();
    }
    public int GetAverageFruit()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        if (villagers.Length <= 0)
        {
            return 0;
        }
        return villagers.Sum(v => v.GetComponent<AgentVillager1>()?.GetFruitCollected() ?? 0) / villagers.Length;
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
            int localLeast = villager.GetComponent<AgentVillager1>().GetFruitCollected();
            if (localLeast < least)
            {
                least = localLeast;
            }
        }
        return least;
    }
    public bool NoFruitBushes()
    {
        return GameObject.FindGameObjectsWithTag("Bush").Length <= 0;
    }

    public bool NoVillagers()
    {
        return GameObject.FindGameObjectsWithTag("Villager").Length <= 0;
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
            int localMost = villager.GetComponent<AgentVillager1>().GetFruitCollected();
            if (localMost > most)
            {
                most = localMost;
            }
        }
        return most;
    }
}
