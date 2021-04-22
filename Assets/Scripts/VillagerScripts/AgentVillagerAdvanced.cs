using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentVillagerAdvanced : AgentVillager1
{
    new GameObject floor;
    public GameObject nearestVillager;
    private float desireToShare;
    private float desireToReveal;
    private float ageOfDeath;
    private float ageCounter;
    private float energyLossRate;
    private bool sharing;
    public bool receivedFruit;
    private bool revealing;
    [SerializeField]
    private int carryingCapacity;
    public bool villagerSeen;
    public Personality personality;     
    public StrengthTrait strengthTrait;
    public SpeedTrait speedTrait;

    private Vector3 CalculateRandomPosition()
    {
        floor = GameObject.Find("Floor");
        Vector3 randomPos = Vector3.zero;
        float floorX = floor.transform.position.x;
        float floorZ = floor.transform.position.z;
        randomPos.x = Random.Range(-floorX, floorX);
        randomPos.z = Random.Range(-floorZ, floorZ);
        return randomPos;
    }
    // Update is called once per frame
    private void ChangeDirection(Vector3 randomPos)
    {
        agent.SetDestination(randomPos);
        switchDirectionCounter = 0;
        transform.LookAt(randomPos);
    }

    private IEnumerator CheckIfSharing()
    {
        yield return new WaitForSeconds(10);
        if (Random.value < desireToShare)
        {
            sharing = true;
        }
        else
        {
            sharing = false;
        }
    }

    private IEnumerator CheckIfReavealing()
    {
        yield return new WaitForSeconds(10);
        if (Random.value < desireToShare)
        {
            revealing = true;
        }
        else
        {
            revealing = false;
        }
    }

    private void CalculateEnergyLossRate()
    {
        if (currentHeldFruit > 0)
        {
            energyLossRate = 1 + (0.05F * currentHeldFruit);
        }
        else
        {
            energyLossRate = 1;
        }
    }

    private IEnumerator loseEnergy()
    {
        yield return new WaitForSeconds(1);
        currentEnergy -= (Time.deltaTime * energyLossRate);
    }


 
    private void Start()
    {
        receivedFruit = false;
        ageCounter = 0;
        ageOfDeath = Random.Range(45, 60);
        currentEnergy = 60;
        energyLossRate = 1;
        sharing = false;
        switch (personality)
        {
            case Personality.Selfish:
                desireToReveal = 0;
                desireToShare = 0;
                break;
            case Personality.Neutral:
                desireToReveal = Random.Range(.25F, .50F);
                desireToShare = Random.Range(.25F, .50F);
                break;
            case Personality.Empathetic:
                desireToReveal = Random.Range(.75F, 1);
                desireToShare = Random.Range(.75F, 1);
                break;                     
        }

        switch (strengthTrait)
        {
            case StrengthTrait.Strong:
                carryingCapacity = Random.Range(6, 9);
                break;
            case StrengthTrait.Regular:
                carryingCapacity = Random.Range(3, 6);
                break;
            case StrengthTrait.Weak:
                carryingCapacity = Random.Range(0, 3);
                break;
            default:
                break;
        }

        switch (speedTrait)
        {
            case SpeedTrait.Fast:
                baseSpeed = Random.Range(9, 12);
                break;
            case SpeedTrait.Regular:
                baseSpeed = Random.Range(6, 9);
                break;
            case SpeedTrait.Slow:
                baseSpeed = Random.Range(3, 6);
                break;
            default:
                break;
        }
        agent.speed = baseSpeed;
        agent.angularSpeed = baseSpeed;
        totalFruitCollected = 0;
        currentHeldFruit = 0;
        motionless = false;
        villager = gameObject;
        currentEnergy = 100F;
        character = GetComponent<CharacterController>();
        agent.acceleration = baseSpeed;
        Vector3 randomPosition = CalculateRandomPosition();
        ChangeDirection(randomPosition);
        if (Random.value < desireToShare)
        {
            sharing = true;
        }
        if (Random.value < desireToReveal)
        {
            revealing = true;
        }


    }

    private void FixedUpdate()
    {
        if (ageCounter >= ageOfDeath)
        {
            Destroy(gameObject);
            return;
        }
        StartCoroutine(CheckIfSharing());
        StartCoroutine(CheckIfReavealing());
        ageCounter += Time.deltaTime;
        CalculateEnergyLossRate();
        StartCoroutine(loseEnergy());
        energyBar.sizeDelta = new Vector2(currentEnergy, energyBar.sizeDelta.y);
        switchDirectionCounter += Time.deltaTime;
        if (currentEnergy <= 0)
        {
            Destroy(gameObject);
            return;
        }
        if (currentHeldFruit > 0)
        {
            if (sharing && !receivedFruit)
            {
                gameObject.tag = "Temp";
                GameObject[] viableVillagers = GameObject.FindGameObjectsWithTag("Villager");
                GameObject dest = viableVillagers[Random.Range(0, viableVillagers.Length)];
                agent.SetDestination(dest.transform.position);
                gameObject.tag = "Villager";
                if (agent.remainingDistance < 2)
                {
                    AgentVillagerAdvanced destAgent = dest.GetComponent<AgentVillagerAdvanced>();
                    destAgent.currentHeldFruit += currentHeldFruit;
                    destAgent.receivedFruit = true;
                    currentHeldFruit = 0;
                    sharing = false;
                }
                StartCoroutine(ChangeToFalse(dest));
            }
            else
            {
                GameObject floorZone = FindObjectOfType<FloorZoneAdvanced>().gameObject;
                bool placed = false;
                agent.SetDestination(floorZone.transform.position);
                if (!placed && gameObject.transform.position.x <= floorZone.transform.position.x + 2 && gameObject.transform.position.z <= floorZone.transform.position.z + 2)
                {
                    StartCoroutine(WaitSeconds(1));
                    FloorZoneAdvanced floor = FindObjectOfType<FloorZoneAdvanced>();
                    floor.PlaceFruit(currentHeldFruit, gameObject.GetComponent<AgentVillagerAdvanced>());
                    totalFruitCollected += currentHeldFruit;
                    currentEnergy += currentHeldFruit * 10;
                    if (currentEnergy > 100)
                    {
                        currentEnergy = 100;
                    }
                    currentHeldFruit = 0;
                    placed = true;
                }
                return;
            }

        }

        if (bushSeen && closestBush != null && closestBush.GetComponent<FruitBush>().visible)
        {
            if (revealing)
            {
                gameObject.tag = "Temp";
                GameObject[] viableVillagers = GameObject.FindGameObjectsWithTag("Villager");
                gameObject.tag = "Villager";
                GameObject neighbour = viableVillagers[Random.Range(0, viableVillagers.Length)];
                if (neighbour.GetComponent<AgentVillagerAdvanced>().closestBush == null)
                {
                    neighbour.GetComponent<AgentVillagerAdvanced>().closestBush = closestBush;
                }
            }
            bool fruitPicked = false;
            agent.SetDestination(closestBush.transform.position);
            transform.LookAt(closestBush.transform);
            if (!fruitPicked && Vector3.Distance(closestBush.transform.position, agent.transform.position) <= 3)
            {
                StartCoroutine(WaitSeconds(1));
                FruitBush closest = closestBush.GetComponent(typeof(FruitBush)) as FruitBush;
                currentHeldFruit = closest.PickFruit(carryingCapacity, currentHeldFruit);
                if (closest.GetTotalFruit() <= 0)
                {
                    closestBush = null;
                    bushSeen = false;
                }
            }
            return;
        }
        if (agent != null)
        {
            if (switchDirectionCounter >= Random.Range(6, 8) || agent.remainingDistance < 1)
            {
                Vector3 randomPosition = CalculateRandomPosition();
                ChangeDirection(randomPosition);
            }
        }
    }
 
    IEnumerator ChangeToFalse(GameObject dest) 
    {
        yield return new WaitForSeconds(10);
        if (dest != null)
        {
            dest.GetComponent<AgentVillagerAdvanced>().receivedFruit = false;
        }
    }
    public enum Personality : int
    {
        Selfish,
        Neutral,
        Empathetic
    }

    public enum StrengthTrait : int
    {
        Strong,
        Regular,
        Weak,
    }

    public enum SpeedTrait 
    {
        Fast,
        Regular,
        Slow
    }
}
