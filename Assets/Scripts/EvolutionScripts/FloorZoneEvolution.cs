using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class FloorZoneEvolution : MonoBehaviour
{
    public TournamentSelection tournament;
    public GameObject floor;
    private int totalFruitCount;
    private int fruitCountOfGeneration;
    private float valueOfFruit;
    private int currentGeneration = 1;
    private List<Vector3> positions;
    float timer = 0;
    float secondsTimer = 0;
    private List<TournamentCandidate> candidates;
    private void Awake()
    {
        candidates = new List<TournamentCandidate>();
        valueOfFruit = 1.5F;
        positions = new List<Vector3>();
        totalFruitCount = 0;
        fruitCountOfGeneration = 0;
    }

    public void InitialSpawning()
    {
        GenerateInitialAgents(20);
    }
    private void GenerateInitialAgents(int villagersToSpawn)
    {
        for (int i = 0; i < villagersToSpawn; i++)
        {
            GameObject spawnPoint = new GameObject();
            spawnPoint.transform.position = gameObject.transform.position;
            spawnPoint.transform.parent = gameObject.transform;
            spawnPoint.transform.localPosition = GenerateCoordinates();
            GameObject villager = (GameObject)Instantiate(Resources.Load("agentVillagerEvolution"), spawnPoint.transform.position, Quaternion.identity);
            AgentVillagerEvolution agent = villager.GetComponent<AgentVillagerEvolution>();
            agent.GenerateInitialTraits();
            villager.name = "Villager" + currentGeneration;
            CapsuleCollider capsuleCollider = villager.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            capsuleCollider.enabled = false;
            Destroy(spawnPoint);
        }
    }

    public int GetFruitCount()
    {
        return totalFruitCount;
    }

    public int GetFruitCountInGeneration()
    {
        return fruitCountOfGeneration;
    }


    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        //secondsTimer += Time.deltaTime;
        if (timer >= 1)
        {
            DecreaseValue(0.05F);
            timer = 0;
        }
        Debug.Log(valueOfFruit);
    }

    void IncreaseValue(float increaseValue)
    {
        valueOfFruit += increaseValue;
    }

    
    void DecreaseValue(float decreaseValue)
    {
        valueOfFruit -= decreaseValue;
    }


    public void ResetFruitCountInGeneration()
    {
        fruitCountOfGeneration = 0;
    }

    public float GetFittestCandidate()
    {
        return candidates.OrderByDescending(x => x.fitness).First().fitness;
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

    private void ProduceOffspringPair(int villagersToSpawn, TournamentSelection tournament, int currentGeneration)
    {
            List<AgentVillagerEvolution> parents = tournament.GetTournamentWinners();
            Crossover crossover = new Crossover(parents[0], parents[1]);

            GameObject spawnPoint = new GameObject();
            spawnPoint.transform.position = gameObject.transform.position;
            spawnPoint.transform.parent = gameObject.transform;
            spawnPoint.transform.localPosition = GenerateCoordinates();

            GameObject spawnPoint2 = new GameObject();
            spawnPoint2.transform.position = gameObject.transform.position;
            spawnPoint2.transform.parent = gameObject.transform;
            spawnPoint2.transform.localPosition = GenerateCoordinates();

            GameObject offspringOne = (GameObject)Instantiate(Resources.Load("agentVillagerEvolution"), spawnPoint.transform.position, Quaternion.identity);
            GameObject offspringTwo = (GameObject)Instantiate(Resources.Load("agentVillagerEvolution"), spawnPoint2.transform.position, Quaternion.identity);
            AgentVillagerEvolution offSpringOneAgent = offspringOne.GetComponent<AgentVillagerEvolution>();
            offSpringOneAgent.name = "Villager" + (currentGeneration + 1);    
            offSpringOneAgent.GenerateFromAncestor(crossover.offspring[0]);
            AgentVillagerEvolution offSpringTwoAgent = offspringTwo.GetComponent<AgentVillagerEvolution>();
            offSpringTwoAgent.name = "Villager" + (currentGeneration + 1);
            offSpringTwoAgent.GenerateFromAncestor(crossover.offspring[1]);
    }

    public void Reproduce(int currentGeneration)
    {
        Debug.Log(string.Join(",", candidates.Select(x => x.fitness)));
        int newVillagerAmount = 20;
        for (int i = 0; i < newVillagerAmount / 2; i++)
        {
            TournamentSelection tournament = new TournamentSelection(candidates);
            ProduceOffspringPair(newVillagerAmount, tournament, currentGeneration);
        }
        DestroyLastGenerationVillagers(currentGeneration);
        candidates = new List<TournamentCandidate>();
        //secondsTimer = 0;
        timer = 0;
        valueOfFruit = 1.5F;
    }
    public void PlaceFruit(int fruit, AgentVillagerEvolution agent)
    {
        if (fruit > 0)
        {
            totalFruitCount += fruit;
            fruitCountOfGeneration += fruit;
            if (!candidates.Any(t => t.candidate == agent))
            {
                candidates.Add(new TournamentCandidate(agent, fruit * valueOfFruit));
            }
            else
            {
                TournamentCandidate candidate = candidates.Where(x => x.candidate == agent).FirstOrDefault();
                candidate.fitness += fruit * valueOfFruit;
            }
        }
    }

    private Vector3 GenerateCoordinates()
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
}
