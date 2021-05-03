using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentVillagerEvolution : AgentVillager1
{
    new GameObject floor;
    public GameObject nearestVillager;
    public RectTransform deadEnergy;
    public List<float> traits = new List<float>();
    public float desireToShare;
    public float desireToReveal;
    public float speed;
    public float baseEnergyLossRate;
    private float energyLossRate;
    private bool sharing;
    private int totalFruitCollected;
    public bool receivedFruit;
    private bool revealing;
    public int carryingCapacity;
    public bool villagerSeen;

    private void Start()
    {
        currentHeldFruit = 0;
        totalFruitCollected = 0;
        energyLossRate = 1;
    }

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
    private void ChangeDirection(Vector3 randomPos)
    {
        agent.SetDestination(randomPos);
        switchDirectionCounter = 0;
        transform.LookAt(randomPos);
    }

    IEnumerator ChangeToFalse(GameObject dest)
    {
        yield return new WaitForSeconds(5);
        if (dest != null)
        {
            dest.GetComponent<AgentVillagerEvolution>().receivedFruit = false;
        }
    }
    private IEnumerator CheckIfSharing()
    {
        yield return new WaitForSeconds(5);
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
        yield return new WaitForSeconds(5);
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
            energyLossRate = baseEnergyLossRate + (0.03F * currentHeldFruit) + (speed / 10) - 1;
        }
        else
        {
            energyLossRate = baseEnergyLossRate + (speed / 10 - 1);
        }
    }
    private IEnumerator loseEnergy()
    {
        yield return new WaitForSeconds(1);
        currentEnergy -= (Time.deltaTime * energyLossRate);
    }

    public void GenerateInitialTraits()
    {
        desireToReveal = Random.Range(-10, 11) + 50;
        desireToShare = Random.Range(-10, 11) + 50;
        currentEnergy = Random.Range(-10, 11) + 60;
        baseEnergyLossRate = (float) System.Math.Round(Random.Range(-0.2F, 0.3F) + 1, 3);
        deadEnergy.sizeDelta = new Vector2(currentEnergy, deadEnergy.sizeDelta.y);
        energyBar.sizeDelta = new Vector2(currentEnergy, energyBar.sizeDelta.y);
        agent.speed = Random.Range(-3, 4) + 10;
        speed = agent.speed;
        carryingCapacity = Random.Range(-2, 3) + 3;
        agent.acceleration = agent.speed;
        traits.Add(desireToReveal);
        traits.Add(desireToShare);
        traits.Add(currentEnergy);
        traits.Add(baseEnergyLossRate);
        traits.Add(speed);
        traits.Add(carryingCapacity);
    }

    public void GenerateFromAncestor(List<float> traits)
    {
        desireToReveal = traits[0];
        desireToShare = traits[1];
        currentEnergy = traits[2];
        baseEnergyLossRate = traits[3];
        deadEnergy.sizeDelta = new Vector2(currentEnergy, deadEnergy.sizeDelta.y);
        energyBar.sizeDelta = new Vector2(currentEnergy, energyBar.sizeDelta.y);
        agent.speed = 0;
        agent.speed = traits[4];
        speed = agent.speed;
        carryingCapacity = (int)traits[5];
        this.traits.Add(desireToReveal);
        this.traits.Add(desireToShare);
        this.traits.Add(currentEnergy);
        this.traits.Add(baseEnergyLossRate);
        this.traits.Add(speed);
        this.traits.Add(carryingCapacity);
    }


    private void FixedUpdate()
    {
        StartCoroutine(CheckIfSharing());
        StartCoroutine(CheckIfReavealing());
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
                if (viableVillagers.Length < 1)
                {
                    sharing = false;
                }
                else
                {
                    GameObject dest = viableVillagers[Random.Range(0, viableVillagers.Length)];
                    agent.SetDestination(dest.transform.position);
                    if (agent.remainingDistance < 2)
                    {
                        AgentVillagerEvolution destAgent = dest.GetComponent<AgentVillagerEvolution>();
                        destAgent.currentHeldFruit += currentHeldFruit;
                        destAgent.receivedFruit = true;
                        currentHeldFruit = 0;
                        sharing = false;
                    }
                    StartCoroutine(ChangeToFalse(dest));
                }
                gameObject.tag = "Villager";
            }
            else
            {
                GameObject floorZone = FindObjectOfType<FloorZoneEvolution>().gameObject;
                bool placed = false;
                agent.SetDestination(floorZone.transform.position);
                if (!placed && gameObject.transform.position.x <= floorZone.transform.position.x + 2 && gameObject.transform.position.z <= floorZone.transform.position.z + 2)
                {
                    StartCoroutine(WaitSeconds(1));
                    FloorZoneEvolution floor = FindObjectOfType<FloorZoneEvolution>();
                    floor.PlaceFruit(currentHeldFruit, this);
                    totalFruitCollected += currentHeldFruit;
                    currentEnergy += currentHeldFruit * 10;
                    if (currentEnergy > deadEnergy.sizeDelta.x)
                    {
                        currentEnergy = deadEnergy.sizeDelta.x;
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
                if (viableVillagers.Length < 1)
                {
                    revealing = false;
                }
                else
                {
                    GameObject neighbour = viableVillagers[Random.Range(0, viableVillagers.Length)];
                    if (neighbour.GetComponent<AgentVillagerEvolution>().closestBush == null)
                    {
                        neighbour.GetComponent<AgentVillagerEvolution>().closestBush = closestBush;
                    }
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
}
