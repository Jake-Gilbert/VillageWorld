using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AgentVillagerAdvanced;

public class FloorZoneAdvanced : FloorZone
{
    int livingVillagersCount;
    static int villagerIndex;
    private new List<Vector3> positions;
    int totalFruitCount;
    int currentFruitSupply;
    private void Awake()
    {
        villagerIndex = 0;
        positions = new List<Vector3>();
    }

    private void Start()
    {
    }
    public void InitialSpawning()
    {
        int villagersToSpawn =  (int) (amountOfAgents * Random.Range(0.5F, 1) + 1);
        GenerateInitialAgents(villagersToSpawn);
    }
    
    private T GetRandomEnum<T>()
    {
        IList<T> enumList = System.Enum.GetValues(typeof(T)).Cast<T>().ToList();
        return enumList[Random.Range(0, enumList.Count() - 1)];
    }
    
    private void GenerateInitialAgents(int villagersToSpawn)
    {
        for (int i = 0; i < villagersToSpawn; i++)
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
            villager.name = "Villager" + (villagerIndex + 1);
            villager.tag = "Villager";
            CapsuleCollider capsuleCollider = villager.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            capsuleCollider.enabled = false;
            villagerIndex++;
        }
    }

    private void ProduceNewGeneration(int amountOfAgents)
    {
       
    }

    public void Reproduce()
    {
        if (livingVillagersCount < 0)
        {
            return;
        }
        else
        {
            //int newVillagerAmount = GetFruitCount() / villagerCount;
            int newVillagerAmount = 5;
            //ProduceNewGeneration(newVillagerAmount);
        }
    }
    private new Vector3 GenerateCoordinates()
    {
        Vector3 coordinates = Vector3.zero;

        if (positions.Count > 0)
        {
            coordinates.x = Random.Range(-0.15F, 0.15F);
            coordinates.z = Random.Range(-0.15F, 0.15F);
            foreach (Vector3 existingPosition in positions)
            {
                if (existingPosition.x - coordinates.x > 0.25F && existingPosition.z - coordinates.z > 0.25F)
                {
                    coordinates.x += 0.25F;
                    coordinates.z += 0.25F;
                }
            }
        }
        else
        {
            coordinates.x = Random.Range(-0.15F, 0.15F);
            coordinates.z = Random.Range(-0.15F, 0.15F);
            positions.Add(coordinates);
        }
        coordinates.y = 1F;
        return coordinates;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
