using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorZoneCompetition : MonoBehaviour
{
    public GameObject floor;
    private int village1FruitCount;
    private int village2FruitCount;
    private float valueOfFruit;
    private int currentGeneration = 1;
    private List<Vector3> positions;
    bool initialised = false;
    float timer = 0;
    float secondsTimer = 0;
    private void Awake()
    {
        valueOfFruit = 1.5F;
        positions = new List<Vector3>();
        village1FruitCount = 0;
        village2FruitCount = 0;
}

    public void InitialSpawning()
    {
        GenerateInitialVillager1Agents(25, 25);
        //GenerateInitialVillager2Agents(25, 25);
    }
    private void GenerateInitialVillager1Agents(int passiveAgents, int aggressiveAgents)
    {
        for (int i = 0; i < passiveAgents; i++)
        {
            GameObject spawnPoint = new GameObject();
            spawnPoint.transform.position = floor.transform.position;
            spawnPoint.transform.parent = floor.transform;
            spawnPoint.transform.localPosition = GenerateCoordinates();
            GameObject villager = (GameObject)Instantiate(Resources.Load("agentVillagerCompetitive"), spawnPoint.transform.position, Quaternion.identity);
            AgentVillagerCompetitive agent = villager.GetComponent<AgentVillagerCompetitive>();
            agent.village = AgentVillagerCompetitive.Village.village1;
            agent.personality = AgentVillagerCompetitive.Personality.passive;
            villager.name = "Villager" + currentGeneration;
            CapsuleCollider capsuleCollider = villager.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            capsuleCollider.enabled = false;
            Destroy(spawnPoint);
        }
        for (int i = 0; i < aggressiveAgents; i++)
        {
            GameObject spawnPoint = new GameObject();
            spawnPoint.transform.position = floor.transform.position;
            spawnPoint.transform.parent = floor.transform;
            spawnPoint.transform.localPosition = GenerateCoordinates();
            GameObject villager = (GameObject)Instantiate(Resources.Load("agentVillagerCompetitive"), spawnPoint.transform.position, Quaternion.identity);
            AgentVillagerCompetitive agent = villager.GetComponent<AgentVillagerCompetitive>();
            agent.village = AgentVillagerCompetitive.Village.village1;
            agent.personality = AgentVillagerCompetitive.Personality.aggressive;
            villager.name = "Villager" + currentGeneration;
            CapsuleCollider capsuleCollider = villager.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            capsuleCollider.enabled = false;
            Destroy(spawnPoint);
        }
    }

    //public int GetFruitCount()
    //{
    //  //  return totalFruitCount;
    //}

    //public int GetFruitCountInGeneration()
    //{
    //  //  return fruitCountOfGeneration;
    //}


    private void FixedUpdate()
    {
        if (!initialised)
        {
            floor = GameObject.Find("Floor");
            initialised = true;
        }
        timer += Time.deltaTime;
        if (timer >= 1)
        {
       //     DecreaseValue(0.05F);
            timer = 0;
        }
    }

    public void ResetFruitCountInGeneration()
    {
       // fruitCountOfGeneration = 0;
    }
    public void DestroyLastGenerationVillagers(int currentGeneration)
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        foreach (GameObject villager in villagers)
        {
            if (villager.name.Contains(currentGeneration.ToString()))
            {
                Destroy(villager);
            }
        }
    }


    public void Reproduce(int currentGeneration)
    {
        //Debug.Log(string.Join(",", candidates.Select(x => x.fitness)));
        //int newVillagerAmount = 20;
        //for (int i = 0; i < newVillagerAmount / 2; i++)
        //{
        //    TournamentSelection tournament = new TournamentSelection(candidates);
        //    ProduceOffspringPair(newVillagerAmount, tournament, currentGeneration);
        //}
        //DestroyLastGenerationVillagers(currentGeneration);
        //candidates = new List<TournamentCandidate>();
        ////secondsTimer = 0;
        //timer = 0;
        //valueOfFruit = 1.5F;
    }
    public void PlaceFruit(int fruit, AgentVillagerCompetitive agent)
    {
        if (fruit > 0)
        {
            if (agent.village == AgentVillagerCompetitive.Village.village1)
            {

            }
            //fruitCountVillage1 += fruit;
            //fruitCoutnVillage2 += fruit;
            //if (!candidates.any(t => t.candidate == agent))
            //{
            //    candidates.add(new tournamentcandidate(agent, fruit * valueoffruit));
            //}
            //else
            //{
            //    tournamentcandidate candidate = candidates.where(x => x.candidate == agent).firstordefault();
            //    candidate.fitness += fruit * valueoffruit;
            //}
        }
    }

    private Vector3 GenerateCoordinates()
    {
        Vector3 coordinates = Vector3.zero;
        coordinates.x = Random.Range(-floor.transform.localScale.x, floor.transform.localScale.x);
        coordinates.z = Random.Range(-floor.transform.localScale.x, floor.transform.localScale.x);
        foreach (Vector3 existingPosition in positions)
        {
            if (existingPosition.x - coordinates.x > 0.25F && existingPosition.z - coordinates.z > 0.25F)
            {
                coordinates.x += 0.25F;
                coordinates.z += 0.25F;
            }
            coordinates.y = 1F;
        }
        return coordinates;
        }
    }
