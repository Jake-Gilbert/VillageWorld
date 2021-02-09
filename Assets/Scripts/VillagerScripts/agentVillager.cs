using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agentVillager : MonoBehaviour
{
    float timer = 0.0F;
    public float speed = 25.0F;
    private Vector3 moveDirection = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {       
            CharacterController controller = GetComponent<CharacterController>();
            System.Random random = new System.Random();
            int randomX = 2;
            int randomZ = 0;
            GameObject floor2 = FindObjectOfType<GameObject>();
        //    FruitBushController floor =  FindObjectOfType<FruitBushController>();
            //MeshRenderer meshRenderer = floor.GetComponent<MeshRenderer>();
            //if (this.transform.localPosition.x  == floor.transform.localScale.x && this.transform.localPosition.z == floor.transform.localScale.z) 
            //{
            //    moveDirection *= -1;
           // }
                
            if (randomX != 0)
            {
                moveDirection = new Vector3(randomX, 0, 0);
            }
            else
            {
                moveDirection = new Vector3(0, 0, randomZ);
            }
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            controller.Move(moveDirection * Time.deltaTime);
            }

    
}
