using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorZone : MonoBehaviour
{
    public GameObject floorZone;
    public GameObject floor;
    List<GameObject> villagers;
    public int fruitCount = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Fruit"))
        {
            fruitCount++;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        villagers = new List<GameObject>();
        //floorZone.transform.parent = floor.transform;
        //floorZone.transform = new Vector3(0.3F, 1F, 0.3F);
        
        int random = Random.Range(3, 6);
        for (int i = 0; i < random; i++)
        {
            GameObject villager = null;
            while (villager == null)
            {
                float[] coordinates = GenerateCoordinates();
                Debug.Log(coordinates[0] + "+ " + coordinates[1]);
                if (PositionIsAvailable(coordinates[0], coordinates[1], villagers))
                {
                    villager = (GameObject)Instantiate(Resources.Load("agentVillager"), new Vector3(0, 0, coordinates[1]) , Quaternion.identity);
                    villager.transform.parent = floorZone.transform;
                    villager.transform.position = new Vector3(floorZone.transform.position.x, 1F, floorZone.transform.position.z);
                   
                    villagers.Add(villager);
                }

            }
        }
    }

        float[] GenerateCoordinates()
        {
            float[] coordinates = new float[2];
            coordinates[0] =  Random.Range(-0.5F, 0.5F);
            coordinates[1] =  Random.Range(-0.5F, 0.5F);          
            return coordinates;
        }

        public bool PositionIsAvailable(float x, float z, List<GameObject> villagers)
        {
            foreach (GameObject villager in villagers)
            {
                if (villager.transform.localPosition.x == x || villager.transform.localPosition.z == z)
                {
                    return false;
                }
            }
            return true;
        }


}
