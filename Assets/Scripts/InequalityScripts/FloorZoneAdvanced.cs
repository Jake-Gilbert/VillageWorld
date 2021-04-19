using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AgentVillagerAdvanced;

public class FloorZoneAdvanced : FloorZone
{
    [SerializeField]
    private ReproductionWithBias reproductionWithBias;
    int livingVillagersCount;
    static int villagerIndex;
    private new List<Vector3> positions;
    private SortedList<Personality, int> dominantPersonality;
    private SortedList<StrengthTrait, int> dominantStrength;
    private SortedList<SpeedTrait, int> dominantSpeed;
    int totalFruitCount;
    int currentFruitSupply;
    private void Awake()
    {
        villagerIndex = 0;
        positions = new List<Vector3>();
        dominantPersonality = new SortedList<Personality, int>();
        dominantPersonality.Add(Personality.Empathetic, 0);
        dominantPersonality.Add(Personality.Neutral, 0);
        dominantPersonality.Add(Personality.Selfish, 0);

        dominantStrength = new SortedList<StrengthTrait, int>();
        dominantStrength.Add(StrengthTrait.Strong, 0);
        dominantStrength.Add(StrengthTrait.Regular, 0);
        dominantStrength.Add(StrengthTrait.Weak, 0);

        dominantSpeed = new SortedList<SpeedTrait, int>();
        dominantSpeed.Add(SpeedTrait.Fast, 0);
        dominantSpeed.Add(SpeedTrait.Regular, 0);
        dominantSpeed.Add(SpeedTrait.Slow, 0);
    }

    private void Start()
    {
    }
    public void InitialSpawning()
    {
        int villagersToSpawn = (int)(amountOfAgents * Random.Range(0.5F, 1) + 1);
        GenerateInitialAgents(villagersToSpawn);
    }

    private T GetRandomEnum<T>()
    {
        IList<T> enumList = System.Enum.GetValues(typeof(T)).Cast<T>().ToList();
        return enumList[Random.Range(0, enumList.Count())];
    }
    private T GetRandomOffspringTrait<T>(SortedList<T, int> traits)
    {
        SortedList<T, int> validTraits = new SortedList<T, int>();
        foreach (int quantity in traits.Values)
        {
            if (quantity > 0)
            {
                validTraits.Add(traits.Keys[traits.IndexOfValue(quantity)], quantity);
                Debug.Log("quantity : " + quantity + " pers : " + traits.Keys[traits.IndexOfValue(quantity)]);
            }
        }
        if (validTraits.Count > 1)
        {
            return validTraits.Keys[Random.Range(0, traits.Count)];
        }
        return validTraits.Keys[0];
    }
    public new int GetFruitCount()
    {
        return totalFruitCount;
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
            livingVillagersCount++;
            villagerIndex++;
        }
    }
    private void GenerateOffSpring(SortedList<Personality, int> personalites, SortedList<StrengthTrait, int> strengths, SortedList<SpeedTrait, int> speeds)
    {
        int villagersToGenerate = personalites.Count;
        Debug.Log("Personaltiy dist : " + string.Join(", ", personalites));
        Debug.Log("Strenght dist : " + string.Join(", ", strengths));
        Debug.Log("Speed dist: : " + string.Join(", ", speeds));
        while (villagersToGenerate > 0)
        {
            villagersToGenerate--;
            GameObject spawnPoint = new GameObject();
            spawnPoint.transform.position = gameObject.transform.position;
            spawnPoint.transform.parent = gameObject.transform;
            spawnPoint.transform.localPosition = GenerateCoordinates();
            GameObject villager = (GameObject)Instantiate(Resources.Load("agentVillager"), spawnPoint.transform.position, Quaternion.identity);
            if (personalites.Values.Sum() > 0)
            {
                Personality offspringPersonality = GetRandomOffspringTrait(personalites);
                villager.GetComponent<AgentVillagerAdvanced>().personality = offspringPersonality;
                personalites[offspringPersonality] -= 1;
            }
            if (personalites.Values.Sum() > 0)
            {
                StrengthTrait offspringPersonality = GetRandomOffspringTrait(strengths);
                villager.GetComponent<AgentVillagerAdvanced>().strengthTrait = offspringPersonality;
                strengths[offspringPersonality] -= 1;
            }
            if (personalites.Values.Sum() > 0)
            {
                SpeedTrait offspringPersonality = GetRandomOffspringTrait(speeds);
                villager.GetComponent<AgentVillagerAdvanced>().speedTrait = offspringPersonality;
                speeds[offspringPersonality] -= 1;
            }
            villager.GetComponent<AgentVillagerAdvanced>().floor = floor;
            villager.name = "Villager" + (villagerIndex + 1);
            villager.tag = "Villager";
            CapsuleCollider capsuleCollider = villager.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            capsuleCollider.enabled = false;
            livingVillagersCount++;
            villagerIndex++;
        }
    }
    public void PlaceFruit(int fruit, AgentVillagerAdvanced agent)
    {
        if (fruit > 0)
        {
            totalFruitCount += fruit;
            dominantPersonality[agent.personality] += 1;
            dominantSpeed[agent.speedTrait] += 1;
            dominantStrength[agent.strengthTrait] += 1;
        }
    }

    public void SpawnInVillagers()
    {
        GameObject spawnPoint = new GameObject();
        spawnPoint.transform.position = gameObject.transform.position;
        spawnPoint.transform.parent = gameObject.transform;
        spawnPoint.transform.localPosition = GenerateCoordinates();
        GameObject villager = (GameObject)Instantiate(Resources.Load("agentVillager"), spawnPoint.transform.position, Quaternion.identity);
        villager.GetComponent<AgentVillagerAdvanced>().personality = GetDominantPersonality();
        villager.GetComponent<AgentVillagerAdvanced>().strengthTrait = GetDominantStrengthTrait();
        villager.GetComponent<AgentVillagerAdvanced>().speedTrait = GetDominantSpeedTrait();
        villager.GetComponent<AgentVillagerAdvanced>().floor = floor;
        villager.name = "Villager" + (villagerIndex + 1);
        villager.tag = "Villager";
        CapsuleCollider capsuleCollider = villager.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
        capsuleCollider.enabled = false;
        livingVillagersCount++;
        villagerIndex++;
    }
  
    public void Reproduce()
    {
        if (livingVillagersCount < 0)
        {
            return;
        }
        else
        {
            if (GetFruitCount() < 0)
            {
                Debug.Log("Population can't grow");
                return;
            }
            int newVillagerAmount = GetFruitCount() / livingVillagersCount;
            reproductionWithBias.ProduceNewGeneration(newVillagerAmount, dominantPersonality, dominantSpeed, dominantStrength);
            GenerateOffSpring(reproductionWithBias.getPersonalityDistribution(), reproductionWithBias.getStrengthDistribution(), reproductionWithBias.getSpeedtDistribution());
            Debug.Log("alive: " + livingVillagersCount);
        }
    }

    public Personality GetDominantPersonality()
    {
        Debug.Log(string.Join(",", dominantPersonality));
        Debug.Log(dominantPersonality.Keys.Count);
        int max = dominantPersonality.Values.Max();
        List<Personality> maxValuesIndexes = dominantPersonality.Keys.Where(k => dominantPersonality[k] == max).ToList();
        return (maxValuesIndexes.Count > 1) ? maxValuesIndexes[Random.Range(0, maxValuesIndexes.Count)] : maxValuesIndexes[0];
    }

    public StrengthTrait GetDominantStrengthTrait()
    {
        Debug.Log(string.Join(",", dominantStrength));
        Debug.Log(dominantStrength.Keys.Count);
        int max = dominantStrength.Values.Max();
        List<StrengthTrait> maxValuesIndexes = dominantStrength.Keys.Where(k => dominantStrength[k] == max).ToList();
        return dominantStrength.Keys[dominantStrength.IndexOfValue(dominantStrength.Values.Max())];
    }

    public SpeedTrait GetDominantSpeedTrait()
    {
        Debug.Log(string.Join(",", dominantSpeed));
        int max = dominantSpeed.Values.Max();
        List<SpeedTrait> maxValuesIndexes = dominantSpeed.Keys.Where(k => dominantSpeed[k] == max).ToList();
        return dominantSpeed.Keys[dominantSpeed.IndexOfValue(dominantSpeed.Values.Max())];
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
