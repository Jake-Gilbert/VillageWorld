using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AgentVillager1 : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject floor;
    public GameObject villager;
    public GameObject closestBush;
    public RectTransform energyBar;
    public int currentHeldFruit; 
    private int totalFruitCollected;
    public float currentEnergy;
    public float baseSpeed;
    public Vector3 randomPos;
    public bool motionless;
    public bool bushSeen;
    private float switchDirectionCounter;
    private CharacterController character;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Boundary"))
        {
            agent.SetDestination(-hit.gameObject.transform.position);
        }

    }
    private void Start()
    {
        totalFruitCollected = 0;
        currentHeldFruit = 0;
        motionless = false;
        villager = gameObject;
        currentEnergy = 100F;
        character = GetComponent<CharacterController>();
        baseSpeed = 10F;
        switchDirectionCounter = 0;
        ChangeDirection();
    }


    private void ChangeDirection()
    {
        GameObject floor = GameObject.Find("Floor");
        Vector3 randomPos = Vector3.zero;
        float floorX = floor.transform.position.x;
        float floorZ = floor.transform.position.z;
        randomPos.x = Random.Range(-floorX, floorX);
        randomPos.z = Random.Range(-floorZ, floorZ);
        //randomPos = transform.TransformDirection(randomPos);
        agent.SetDestination(randomPos);
        switchDirectionCounter = 0;
        RotateInForwardDirection();
    }

    private void Update()
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
            RotateTowardsPosition(floorZone.transform);
            agent.SetDestination(floorZone.transform.position);
            if (!placed && gameObject.transform.position.x <= floorZone.transform.position.x + 2 && gameObject.transform.position.z <= floorZone.transform.position.z + 2)
            {
                StartCoroutine(WaitSeconds(1));
                FloorZone floor = FindObjectOfType<FloorZone>();
                floor.PlaceFruit(currentHeldFruit);
                totalFruitCollected += currentHeldFruit;
                currentHeldFruit = 0;               
                currentEnergy += 10;
                placed = true;             
            }           
            return;
        }

        if (bushSeen && closestBush != null)
        {
            bool fruitPicked = false;
            agent.SetDestination(closestBush.transform.position);
            RotateTowardsPosition(closestBush.transform);
            if (!fruitPicked && Vector3.Distance(closestBush.transform.position, agent.transform.position) <= 3)
            {
                StartCoroutine(WaitSeconds(1));
                FruitBush closest = closestBush.GetComponent(typeof(FruitBush)) as FruitBush;                
                currentHeldFruit = closest.PickFruit();
                fruitPicked = false;
            }
            return;
        }

        if (agent != null)
        {
            if (switchDirectionCounter >= (Random.Range(6, 8)))
            {
                ChangeDirection();
            }
        }

      


    }

    private void RotateTowardsPosition(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5F);

    }

    private void RotateInForwardDirection()
    {
        Vector3 direction = transform.forward;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5F);
    }

    private IEnumerator WaitSeconds(int seconds)
    {
        agent.isStopped = true;      
        yield return new WaitForSeconds(seconds);
        agent.isStopped = false;
    }

    public int GetFruitCollected()
    {
        return totalFruitCollected;
    }
  



}


