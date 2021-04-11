using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AgentVillagerAdvanced;

public class FloorZoneAdvanced : FloorZone
{
    int villagerCount;
    private void Start()
    {
        positions = new List<Vector3>();
    }
    public void InitialSpawning()
    {
        int villagersToSpawn =  (int) (villagerCount * Random.Range(0.7F, 1)) + 1;
        ProduceNewGeneration(villagersToSpawn);
    }
    
    private T GetRandomEnum<T>()
    {
        IList<T> enumList = System.Enum.GetValues(typeof(T)).Cast<T>().ToList();
        return enumList[Random.Range(0, enumList.Count() - 1)];
    }

    private void ProduceNewGeneration(int amountOfAgents)
    {
        for (int i = 0; i < amountOfAgents; i++)
        {
            GameObject spawnPoint = new GameObject();
            spawnPoint.transform.position = gameObject.transform.position;
            spawnPoint.transform.parent = gameObject.transform;
            spawnPoint.transform.localPosition = GenerateCoordinates();
            GameObject villager = (GameObject)Instantiate(Resources.Load("agentVillager"), spawnPoint.transform.position, Quaternion.identity);
            villager.GetComponent<AgentVillagerAdvanced>().personality = GetRandomEnum<Personality>();
            villager.GetComponent<AgentVillagerAdvanced>().strengthTrait = GetRandomEnum<StrengthTrait>();
            villager.GetComponent<AgentVillagerAdvanced>().speedTrait = GetRandomEnum<SpeedTrait>();
            villager.GetComponent<AgentVillagerAdvanced>().floor = floor;
            villager.name = "Villager" + (i + 1);
            villager.tag = "Villager";
            CapsuleCollider capsuleCollider = villager.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            capsuleCollider.enabled = false;
        }
    }

    public void Reproduce()
    {
        if (villagerCount < 0)
        {
            return;
        }
        else
        {
            //int newVillagerAmount = GetFruitCount() / villagerCount;
            int newVillagerAmount = 5;
            ProduceNewGeneration(newVillagerAmount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
