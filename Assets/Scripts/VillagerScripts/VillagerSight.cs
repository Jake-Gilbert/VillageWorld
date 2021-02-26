using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerSight : MonoBehaviour
{
    public GameObject nearestBush;
    public AgentVillager villager;

    // Update is called once per frame
    void Update()
    {
        if(villager == null)
        {
            villager = FindObjectOfType<AgentVillager>();
            Debug.Log(villager);
        }
        RaycastHit hit;             
        Ray lineOfSight = new Ray(villager.transform.position, villager.moveDirection);
        Debug.DrawRay(villager.transform.position, villager.moveDirection);
        if (Physics.Raycast(lineOfSight, out hit, 20))
        {
            if (hit.collider.tag == "Bush" && nearestBush == null)
            {
                nearestBush = hit.collider.gameObject;              
                villager.closestBush = nearestBush;
                FindObjectOfType<VillagerCollision>().nearestBush = nearestBush;
                villager.bushSeen = true;
                
                //         nearestBush = null;
            }
            //nearestBush = null;

        }
    }
}
