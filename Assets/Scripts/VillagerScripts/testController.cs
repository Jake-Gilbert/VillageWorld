using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class testController : MonoBehaviour
{
    //THIS CLASS WAS USED FOR OBSERVATION AND IS NOT PART OF THE RESEARCH
    private float speed = 75.0F;
    private Vector3 moveDirection = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = Vector3.up;
    }
    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        SphereCollider sphereCollider = GetComponentInChildren<SphereCollider>();
        CharacterController controller = GetComponent<CharacterController>();
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");
        moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        controller.Move(moveDirection * Time.deltaTime);
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    GameObject[] bushes = GameObject.FindGameObjectsWithTag("Bush");
        //    foreach(GameObject bush in bushes)
        //    {
        //        Destroy(bush);
        //    }
        //}
        if (Input.GetKey(KeyCode.Q))
        {
                transform.Rotate(Vector3.up * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
                 transform.Rotate(-Vector3.up * speed * Time.deltaTime);
        }
    }
}
