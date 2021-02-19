using UnityEngine;

public class VillagerCollision : MonoBehaviour
{
    public int fruitCount = 0;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {

        if(collision.collider.CompareTag("FruitBush"))
        {
            agentVillager villager = GetComponentInParent(typeof(agentVillager)) as agentVillager;
        }

        if (collision.collider.CompareTag("TouchingBush"))
        {
            Debug.Log("HGe");

            fruitCount++;
            Destroy(collision.gameObject);
            
        }

        if (collision.collider.CompareTag("Boundary"))
        {
            agentVillager villager = GetComponentInParent(typeof(agentVillager)) as agentVillager;
            villager.moveDirection *= -1;
                  
         
        }


    

    }

    // Update is called once per frame
    void Update()
    {
        if (fruitCount > 0 && Input.GetKey(KeyCode.Alpha2))
        {
            //Fruit fruit = (Fruit) Instantiate(Resources.Load("Fruit"), new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z), Quaternion.identity);
            GameObject fruit = (GameObject) Instantiate(Resources.Load("Fruit"), new Vector3(this.transform.parent.localPosition.x, this.transform.parent.localPosition.y, this.transform.parent.localPosition.z), Quaternion.identity);
            fruit.tag = "Fruit";
            Rigidbody fruitRigi = fruit.GetComponent(typeof(Rigidbody)) as Rigidbody;
            fruitRigi.useGravity = true;
            fruitCount--;
            Debug.Log("fruit");
        }
    }
}
