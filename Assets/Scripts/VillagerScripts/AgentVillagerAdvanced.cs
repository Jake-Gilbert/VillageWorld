using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentVillagerAdvanced : AgentVillager1
{
    new GameObject floor;
    private float desireToShare;
    private float desireToReveal;
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
        RotateInForwardDirection();
    }

    private void Start()
    {       
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
                break;
            case StrengthTrait.Regular:
                break;
            case StrengthTrait.Weak:
                break;
            default:
                break;
        }

        switch (speedTrait)
        {
            case SpeedTrait.Fast:
                baseSpeed = 10;
                break;
            case SpeedTrait.Regular:
                baseSpeed = 5;
                break;
            case SpeedTrait.Slow:
                baseSpeed = 1;
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
        baseSpeed = 5F;
        agent.acceleration = baseSpeed;
        Vector3 randomPosition = CalculateRandomPosition();
        ChangeDirection(randomPosition);

        Debug.Log(personality.ToString());
        Debug.Log(strengthTrait.ToString());
        Debug.Log(speedTrait.ToString());
    }

    private void FixedUpdate()
    {
        switchDirectionCounter += Time.deltaTime;

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
        Selfish,
        Neutral,
        Empathetic
    }

    public enum StrengthTrait
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
