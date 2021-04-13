using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerSight : MonoBehaviour
{
    public GameObject nearestBush;
    public AgentVillagerAdvanced villager;


    private void FixedUpdate()
    {
        RaycastHit hit;             
        Ray lineOfSight = new Ray(villager.transform.position, villager.transform.forward);
        Debug.DrawRay(villager.transform.position, villager.transform.forward);
        if (Physics.Raycast(lineOfSight, out hit, 20))
        {
            if (hit.collider.CompareTag("Bush") && nearestBush == null) 
            {
                villager.closestBush = hit.collider.gameObject;
                villager.bushSeen = true;
                nearestBush = null;
            }
        
        }
    }
    

}
