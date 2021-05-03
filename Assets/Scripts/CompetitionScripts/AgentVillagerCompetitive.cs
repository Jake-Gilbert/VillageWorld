using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentVillagerCompetitive : AgentVillager1
{
    new GameObject floor;
    public GameObject nearestVillager;
    public Personality personality;
    public Material aggressive1;
    public Material passive1;
    public Material aggressive2;
    public Material passive2;
    public Village village;
    public bool villagerSeen;

    private void Start()
    {
        if (personality == Personality.passive)
        {
            if (village == Village.village1)
            {
                gameObject.GetComponent<MeshRenderer>().material = passive1;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material = passive2;
            }
        }
        else
        {
            if (village == Village.village1)
            {
                gameObject.GetComponent<MeshRenderer>().material = aggressive1;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material = aggressive2;
            }
        }
        currentHeldFruit = 0;
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

    private void FixedUpdate()
    {
        switchDirectionCounter += Time.deltaTime;

        if (currentHeldFruit > 0)
        {      
                GameObject floorZone = FindObjectOfType<FloorZoneCompetition>().gameObject;
                bool placed = false;
                agent.SetDestination(floorZone.transform.position);
                if (!placed && gameObject.transform.position.x <= floorZone.transform.position.x + 2 && gameObject.transform.position.z <= floorZone.transform.position.z + 2)
                {
                    StartCoroutine(WaitSeconds(1));
                    FloorZoneCompetition floor = FindObjectOfType<FloorZoneCompetition>();
                    floor.PlaceFruit(currentHeldFruit, this);
                    currentHeldFruit = 0;
                    placed = true;
                }
                return;
        }
        if (bushSeen && closestBush != null && closestBush.GetComponent<FruitBush>().visible)
        { 
            bool fruitPicked = false;
            agent.SetDestination(closestBush.transform.position);
            transform.LookAt(closestBush.transform);
            if (!fruitPicked && Vector3.Distance(closestBush.transform.position, agent.transform.position) <= 3)
            {
                StartCoroutine(WaitSeconds(1));
                FruitBush closest = closestBush.GetComponent(typeof(FruitBush)) as FruitBush;
                currentHeldFruit = closest.PickFruit(1, currentHeldFruit);
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

    public enum Personality 
    {
        aggressive,
        passive
    }

    public enum Village 
    {
        village1,
        village2
    }





}
