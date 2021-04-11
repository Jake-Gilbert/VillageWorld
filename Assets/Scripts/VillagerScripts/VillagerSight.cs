using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerSight : MonoBehaviour
{
    public GameObject nearestBush;
    public AgentVillager1 villager;

    private void FixedUpdate()
    {
        RaycastHit hit;             
        Ray lineOfSight = new Ray(villager.transform.position, villager.transform.forward);
        Debug.DrawRay(villager.transform.position, villager.transform.forward);
        if (Physics.Raycast(lineOfSight, out hit, 20))
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
