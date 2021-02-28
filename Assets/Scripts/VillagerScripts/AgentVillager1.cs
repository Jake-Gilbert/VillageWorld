using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AgentVillager1 : MonoBehaviour
{
    private GameObject floorZone;
    public GameObject villager;
    public GameObject closestBush;
    public RectTransform energyBar;
    public int currentHeldFruit;
    private int totalFruitCollected;//TODO
    public float currentEnergy;
    public float baseSpeed;
    public Vector3 moveDirection;
    public bool motionless;
    public bool bushSeen;
    private bool carryingFruit;
    private float switchDirectionCounter;
    private CharacterController character;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Boundary"))
        {
            moveDirection *= -1;
        }

    }
    void Start()
    {
        currentHeldFruit = 0;
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

        moveDirection.z = (baseSpeed) * (Random.value > 0.5 ? 1 : -1);
        moveDirection.x = (baseSpeed) * (Random.value > 0.5 ? 1 : -1);
        moveDirection.y = 0;

        moveDirection = transform.TransformDirection(moveDirection);
    }

    void Update()
    {
        switchDirectionCounter += Time.deltaTime;
        currentEnergy -= Time.deltaTime * 2;
        energyBar.sizeDelta = new Vector2(currentEnergy, energyBar.sizeDelta.y);
        if (currentEnergy <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (currentHeldFruit > 0)
        {
            GameObject floorZone = FindObjectOfType<FloorZone>().gameObject;
            bool placed = false;

             gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(floorZone.transform.position.x, 1, floorZone.transform.position.z), Time.deltaTime * baseSpeed);
            if (!placed && gameObject.transform.position.x == floorZone.transform.position.x && gameObject.transform.position.z == floorZone.transform.position.z)
            {
                DelayMotion();
                FloorZone floor = FindObjectOfType<FloorZone>();
                floor.PlaceFruit(currentHeldFruit);
                currentHeldFruit = 0;
                placed = true;
            }
            bushSeen = true;
            return;
        }

        if (bushSeen && closestBush != null)
        {
            bool fruitPicked = false;
            while(Vector3.Distance(gameObject.transform.position, closestBush.transform.position) > 3)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, closestBush.transform.position, Time.deltaTime * baseSpeed);
            }

            if (!fruitPicked)
                {
                StartCoroutine(DelayMotion());
                FruitBush closest = closestBush.GetComponent(typeof(FruitBush)) as FruitBush;
                closest.PickFruit();
                currentHeldFruit++;
                fruitPicked = false;
            }
            bushSeen = false;
            return;
        }

        if (character != null)
        {
            character.Move(moveDirection * Time.deltaTime);
            if (switchDirectionCounter >= Random.Range(3, 8))
            {
                ChangeDirection();
                switchDirectionCounter = 0;
            }

        }

      


    }

    IEnumerator DelayMotion()
    {
        baseSpeed = 0;
        yield return new WaitForSeconds(3);
        StartMotion();
    }

    void StartMotion()
    {
        baseSpeed = 10;       
    }


}


