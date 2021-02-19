using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agentVillager : MonoBehaviour
{
    
    public GameObject floorZone;
    public GameObject villager;
    public GameObject sightSphere;
    public float energy;
    public float baseSpeed;
    public Vector3 moveDirection;
    private float switchDirectionCounter;
    private CharacterController character;


    void Start()
    {
        villager = gameObject;
        sightSphere = villager.transform.GetChild(3).gameObject;
        energy = 50F;
        character = GetComponent<CharacterController>();
        baseSpeed = 10F;
        moveDirection = new Vector3(1, -1, 1);
        moveDirection *= baseSpeed * Time.deltaTime;
        switchDirectionCounter = 0;
        ChangeDirection();

    }


    void ChangeDirection()
    {

            moveDirection.z = ( baseSpeed) * (Random.value > 0.5 ? 1 : -1);
            moveDirection.x = (baseSpeed ) * (Random.value > 0.5 ? 1 : -1);
            moveDirection.y = -1;

        moveDirection = transform.TransformDirection(moveDirection);
    }

    void Update()
    {
        RaycastHit hit;
        Ray lineOfSight = new Ray(villager.transform.position, Vector3.forward);
        energy -= Time.deltaTime;
        Debug.DrawRay(villager.transform.position, moveDirection);
        if (energy <= 0)
        {
            Destroy(gameObject);
        }
        
        if(Physics.Raycast(lineOfSight, out hit, 20))
        {
            if(hit.collider.tag == "Bush")
            {
                villager.transform.position = Vector3.MoveTowards(villager.transform.position, hit.collider.gameObject.transform.position, 20);
            }
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
