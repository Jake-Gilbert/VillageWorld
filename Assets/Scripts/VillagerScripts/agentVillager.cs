using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agentVillager : MonoBehaviour
{
    private const double SWITCH_DIRECTION_CHANCE_X = .5;
    public float baseSpeed;
    public Vector3 moveDirection;
    private float switchDirectionCounter;
    private CharacterController character;

    private void Start()
    {
        baseSpeed = 10F;
        moveDirection = new Vector3(1, 0, 1);
        moveDirection *= baseSpeed * Time.deltaTime;

        switchDirectionCounter = 0;
        ChangeDirection();
        character = GetComponent<CharacterController>();
    }

    void ChangeDirection()
    {

        ////moveDirection = transform.TransformDirection(moveDirection);       
        //moveDirection *= baseSpeed;
        
            moveDirection.z = ( baseSpeed) * (Random.value > 0.5 ? 1 : -1);
            moveDirection.x = (baseSpeed ) * (Random.value > 0.5 ? 1 : -1);
   
  

        moveDirection = transform.TransformDirection(moveDirection);
    }

    void Update()
    {
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
