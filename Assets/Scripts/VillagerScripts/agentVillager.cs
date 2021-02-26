using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentVillager : MonoBehaviour
{
    
    public GameObject floorZone;
    public GameObject villager;
    public GameObject sightSphere;
    public GameObject closestBush;
    public RectTransform energyBar;
    public List<int> fruitCollection;
    public float currentEnergy;
    public float baseSpeed;
    public Vector3 moveDirection;
    public bool motionless;
    public bool bushSeen;
    int fruitHeld;
    private float switchDirectionCounter;
    private CharacterController character;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Boundary"))
        {
            moveDirection *= -1;
        }

        if (hit.gameObject.CompareTag("Bush") && hit.gameObject == closestBush)
        {
            motionless = true;
        }
    }
    void Start()
    {
        fruitHeld = 0;
        fruitCollection = new List<int>();
        motionless = false;
        villager = gameObject;
        currentEnergy = 100F;
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
        currentEnergy -= Time.deltaTime * 2;
        energyBar.sizeDelta = new Vector2(currentEnergy, energyBar.sizeDelta.y);
        if (currentEnergy <= 0)
        {
            Destroy(gameObject);
        }

        if(fruitCollection.Count > 0)
        {
            bool placed = false;
            GameObject floorZone = FindObjectOfType<FloorZone>().gameObject;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, floorZone.transform.position, 1F);
            if (!placed)
            {                                 
                FloorZone floor = FindObjectOfType<FloorZone>();
                floor.PlaceFruit(fruitCollection);
                placed = true;
                fruitCollection = new List<int>();
            }

         


        }

        if (bushSeen)
        {
            while (Vector3.Distance(closestBush.transform.position, gameObject.transform.position) > 1F)
            {
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, closestBush.transform.position, 20);

            }
        }

        if (motionless)
        {
            moveDirection = Vector3.zero;
            //StartMotion();
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
    
    IEnumerator StopMotion()
    {
        moveDirection = Vector3.zero;
        Debug.Log("Starting");
        yield return new WaitForSeconds(3);

    }

    void StartMotion()
    {
         ChangeDirection();
    
    }

 
}
