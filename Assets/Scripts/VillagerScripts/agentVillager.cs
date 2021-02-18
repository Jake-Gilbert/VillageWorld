using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agentVillager : MonoBehaviour
{
    
    private const double SWITCH_DIRECTION_CHANCE_X = .5;
    public GameObject floorZone;
    public GameObject villager;
    public float energy;
    public float baseSpeed;
    public Vector3 moveDirection;
    private float switchDirectionCounter;
    private CharacterController character;

    void Start()
    {
        villager = gameObject;
        energy = 50F;
        character = GetComponent<CharacterController>();
        baseSpeed = 10F;
        moveDirection = new Vector3(1, 0, 1);
        moveDirection *= baseSpeed * Time.deltaTime;
        switchDirectionCounter = 0;
        ChangeDirection();

    }


    void ChangeDirection()
    {

            moveDirection.z = ( baseSpeed) * (Random.value > 0.5 ? 1 : -1);
            moveDirection.x = (baseSpeed ) * (Random.value > 0.5 ? 1 : -1);
            moveDirection.y = 0;

        moveDirection = transform.TransformDirection(moveDirection);
    }

    void Update()
    {

        energy -= Time.deltaTime;

        if (energy <= 0)
        {
            Destroy(gameObject);
        }

        if (character != null)
        {
            character.Move(moveDirection * Time.deltaTime);
        }

        switchDirectionCounter += Time.deltaTime;

        if (switchDirectionCounter >= Random.Range(3, 8))
        {
            ChangeDirection();
            switchDirectionCounter = 0;
        }
    }
}
