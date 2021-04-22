using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerSight : MonoBehaviour
{
    public GameObject nearestBush;
    //public GameObject nearestVillager;
    public AgentVillagerAdvanced villager;


    private void FixedUpdate()
    {
        if (nearestBush != null)
        {
            if (nearestBush.GetComponent<FruitBush>().GetTotalFruit() <= 0)
            {
                nearestBush = null;
            }
        }
        RaycastHit hit;             
        Ray lineOfSight = new Ray(villager.transform.position, villager.transform.forward);
        Debug.DrawRay(villager.transform.position, villager.transform.forward);
        if (Physics.Raycast(lineOfSight, out hit, 20))
        {
            if (hit.collider.CompareTag("Bush") && nearestBush == null) 
            {
                nearestBush = hit.collider.gameObject;
                if (nearestBush.GetComponent<FruitBush>().visible)
                {
                    villager.closestBush = hit.collider.gameObject;
                    villager.bushSeen = true;
                }
            }
            else if(hit.collider.CompareTag("Bush") && nearestBush != null && hit.collider.CompareTag("Bush") != nearestBush)
            {
                GameObject otherBush = hit.collider.gameObject;
                float distanceNewBush = Vector3.Distance(gameObject.transform.position, otherBush.transform.position);
                float distanceOldBush = Vector3.Distance(gameObject.transform.position, nearestBush.transform.position);
                if (distanceNewBush < distanceOldBush)
                {
                    nearestBush = otherBush;
                    villager.closestBush = nearestBush;
                    villager.bushSeen = true;
                }
            
            }
        }
    }
    

}
