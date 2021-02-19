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
        float floorZoneX = floorZone.transform.localPosition.x;
        float floorZoneZ = floorZone.transform.localPosition.z;
        villagers = new List<GameObject>();
        //floorZone.transform = new Vector3(0.3F, 1F, 0.3F);
        float spawnPointX =  0.5F ;
        float spawnPointZ = 0.5F ;


        int random = Random.Range(3, 6);
        for (int i = 0; i < random; i++)
        {
            
            GameObject spawnPoint = new GameObject();
            spawnPoint.transform.position = floorZone.transform.position;
            spawnPoint.transform.parent = floorZone.transform;
            spawnPoint.transform.localPosition = new Vector3(Random.Range(-spawnPointX , spawnPointX), 1F,  Random.Range(-spawnPointZ, spawnPointZ));
            GameObject villager = (GameObject)Instantiate(Resources.Load("agentVillager"),spawnPoint.transform.position , Quaternion.identity);                 
            villagers.Add(villager);
                

            
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
