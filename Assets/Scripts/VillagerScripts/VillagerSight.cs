using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerSight : MonoBehaviour
{
    public GameObject nearestBush;
    public AgentVillager1 villager;

    // Update is called once per frame
    void Update()
    {
      
        RaycastHit hit;             
        Ray lineOfSight = new Ray(villager.transform.position, villager.moveDirection);
        Debug.DrawRay(villager.transform.position, villager.moveDirection);
        if (Physics.Raycast(lineOfSight, out hit, 5))
        {
            if (hit.collider.CompareTag("Bush") && nearestBush == null) 
            {
                GameObject nearestBush = hit.collider.gameObject;
                villager.closestBush = nearestBush;
                villager.bushSeen = true;
                
               
            }
        
        }
    }
}
