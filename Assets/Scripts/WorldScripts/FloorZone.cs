using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorZone : MonoBehaviour
{
    public GameObject floor;
    public int amountOfAgents;
    protected List<Vector3> positions;
    private int fruitCount = 0;

    private void Start()
    {
        positions = new List<Vector3>();
        float floorZoneX = gameObject.transform.localPosition.x;
        float floorZoneZ = gameObject.transform.localPosition.z;

        for (int i = 0; i < amountOfAgents; i++)
        {
            GameObject spawnPoint = new GameObject();
            spawnPoint.transform.position = gameObject.transform.position;
            spawnPoint.transform.parent = gameObject.transform;
            spawnPoint.transform.localPosition = GenerateCoordinates();
            GameObject villager = (GameObject)Instantiate(Resources.Load("agentVillager"), spawnPoint.transform.position, Quaternion.identity);
            villager.name = "Villager" + (i + 1);
            villager.tag = "Villager";
            CapsuleCollider capsuleCollider = villager.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            capsuleCollider.enabled = false;
        }
    }

    public int GetFruitCount()
    {
        return fruitCount;
    }

    public void PlaceFruit(int fruit)
    {
        if (fruit > 0)
        {
            fruitCount += fruit;
        }
    }
    protected Vector3 GenerateCoordinates()
    {
        Vector3 coordinates = Vector3.zero;

        if (positions.Count > 0)
        {
            coordinates.x = Random.Range(-0.5F, 0.5F);
            coordinates.z = Random.Range(-0.5F, 0.5F);
            foreach (Vector3 existingPosition in positions)
            {
                if (existingPosition.x - coordinates.x > 0.25F && existingPosition.z - coordinates.z > 0.25F)
                {
                    coordinates.x += 0.25F;
                    coordinates.z += 0.25F;
                }
            }
        }
        else
        {
            coordinates.x = Random.Range(-0.5F, 0.5F);
            coordinates.z = Random.Range(-0.5F, 0.5F);
            positions.Add(coordinates);
        }
        coordinates.y = 1F;
        return coordinates;
    }

}
