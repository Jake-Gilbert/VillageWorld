using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FruitBush : MonoBehaviour
{
    //public GameObject bush;
    private SphereCollider collider;
    [SerializeField]
    private int numberOfFruit = 0;
    private bool controllerFound = false;
    private bool fruitDepleted = false;
    public bool visible = true;
    [SerializeField]
    private float deathTimer = 0;
    FruitBushControllerAdvanced fruitBushController;
    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        int noOfFruits = Random.Range(3, 7);
        numberOfFruit = noOfFruits;
        for (int i = 0; i < noOfFruits; i++)
        {
            GameObject rotationPoint = new GameObject();
            rotationPoint.transform.position = gameObject.transform.position;
            GameObject fruit = (GameObject)Instantiate(Resources.Load("Fruit"), gameObject.transform.position + new Vector3(0, collider.radius * 2.9F, 0), Quaternion.identity);
            fruit.transform.parent = rotationPoint.transform;
            rotationPoint.transform.eulerAngles = new Vector3(Random.Range(-100, 100), Random.Range(-50, 50), Random.Range(-100, 100));
            fruit.transform.parent = gameObject.transform;
            Destroy(rotationPoint.gameObject);
        }
    }

    public int GetTotalFruit()
    {
        return numberOfFruit;
    }
    public int PickFruit()
    {
        if (numberOfFruit > 0)
        {
            int amountOfFruitTaken = Random.Range(1, numberOfFruit);
            int returnAmount = amountOfFruitTaken;
            for (int i = 0; i < amountOfFruitTaken; i++)
            {
                GameObject fruit = gameObject.transform.GetChild(i).gameObject;
                if (fruit != null)
                {
                    numberOfFruit--;
                    Destroy(fruit);
                }
            }
            return returnAmount;
        }
        return 0;
    }

    public void SpawnFruit()
    {
        int noOfFruits = Random.Range(3, 7);
        numberOfFruit = noOfFruits;
        for (int i = 0; i < noOfFruits; i++)
        {
            GameObject rotationPoint = new GameObject();
            rotationPoint.transform.position = gameObject.transform.position;
            GameObject fruit = (GameObject)Instantiate(Resources.Load("Fruit"), gameObject.transform.position + new Vector3(0, collider.radius * 2.9F, 0), Quaternion.identity);
            fruit.transform.parent = rotationPoint.transform;
            rotationPoint.transform.eulerAngles = new Vector3(Random.Range(-100, 100), Random.Range(-50, 50), Random.Range(-100, 100));
            fruit.transform.parent = gameObject.transform;
            Destroy(rotationPoint.gameObject);
        }
        fruitDepleted = false;
    }

    public int PickFruit(int carryingCapacity, int heldFruit)
    {
        int fruitToPick = 0;
        if (numberOfFruit > 0)
        {
            if (numberOfFruit < carryingCapacity - heldFruit)
            {
                fruitToPick = numberOfFruit;
            }
            else
            {
                fruitToPick = carryingCapacity - heldFruit;
            }
            for (int i = 0; i < fruitToPick; i++)
            {
                GameObject fruit = gameObject.transform.GetChild(i).gameObject;
                if (fruit != null)
                {
                    numberOfFruit--;
                    Destroy(fruit);
                }
            }
            return fruitToPick;
        }
        return 0;
    }

    private void FixedUpdate()
    {
        if (numberOfFruit > 0)
        {
            visible = true;
        }
        if (!controllerFound)
        {
            fruitBushController = FindObjectOfType<FruitBushControllerAdvanced>();
            controllerFound = true;
        }
        if (numberOfFruit <= 0 && !fruitDepleted)
        {
            visible = false;
            gameObject.SetActive(false);
            fruitBushController.depletedBushes.Add(gameObject);
            fruitDepleted = true;
        }
    }

}

