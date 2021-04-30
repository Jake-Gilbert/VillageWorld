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
    private List<Vector3> positions;
    private List<TournamentCandidate> candidates;
    private void Awake()
    {
        candidates = new List<TournamentCandidate>();
        valueOfFruit = 1;
        positions = new List<Vector3>();
        totalFruitCount = 0;
        fruitCountOfGeneration = 0;
    }

    public void InitialSpawning()
    {
        GenerateInitialAgents(10);
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
            villager.name = "Villager" + (i + 1);
            CapsuleCollider capsuleCollider = villager.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            capsuleCollider.enabled = false;
            Destroy(spawnPoint);
        }
    }

    public void Reproduce()
    {
        Debug.Log(string.Join(",", candidates.Where(x => x.fitness > -1).Select(x => x.fitness)));
        TournamentSelection tournament = new TournamentSelection(candidates);
        Debug.Log(string.Join(",", candidates.Where(x => x != null)));
    }
    public void PlaceFruit(int fruit, AgentVillagerEvolution agent)
    {
        if (fruit > 0)
        {
            totalFruitCount += fruit;
            fruitCountOfGeneration += fruit;
            if (!candidates.Any(t => t.candidate == agent))
            {
                candidates.Add(new TournamentCandidate(agent, (int) (fruit * valueOfFruit)));
            }
            else
            {
                TournamentCandidate candidate = candidates.Where(x => x.candidate == agent).FirstOrDefault();
                candidate.fitness += (int) (fruit * valueOfFruit);
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
