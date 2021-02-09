using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    public int sizeX;
    public int sizeZ;
    // Start is called before the first frame update
    void Start()
    {
        GameObject floor = (GameObject)Instantiate(Resources.Load("Floor"), new Vector3(0, 0, 0), Quaternion.identity);
        floor.transform.localPosition = new Vector3(0, -1F, 0);
        floor.transform.localScale = new Vector3(sizeX, 1, sizeZ);
        floor.AddComponent<FloorInteractions>();
        floor.tag = "Boundary";
        System.Random random = new System.Random();

        //Four boundary walls
        BoxCollider north = floor.AddComponent(typeof(BoxCollider)) as BoxCollider;
        north.center = new Vector3(-0.5F, 0, 0);
        north.size = new Vector3(0, 50, 1);

        BoxCollider south = floor.AddComponent(typeof(BoxCollider)) as BoxCollider;
        south.center = new Vector3(0.5F, 0, 0);
        south.size = new Vector3(0, 50, 1);
       

        BoxCollider east = floor.AddComponent(typeof(BoxCollider)) as BoxCollider;
        east.center = new Vector3(0F, 0, 0.5F);
        east.size = new Vector3(1, 50, 0);

        BoxCollider west = floor.AddComponent(typeof(BoxCollider)) as BoxCollider;
        west.center = new Vector3(0F, 0, -0.5F);
        west.size = new Vector3(1, 50, 0);

        float floorX = gameObject.transform.localPosition.x;
        float floorZ = gameObject.transform.localPosition.z;
        GameObject floorzone1 = (GameObject)Instantiate(Resources.Load("CollectionZone"), new Vector3(random.Next(-120, 120), -0.5F, random.Next(-120, 120)), Quaternion.identity);

        float shapeSizeZ = floor.transform.localScale.z / 4;
        float shapeSizeX = floor.transform.localScale.x / 4;

        float hectareZ = -floor.transform.localScale.z/2 + (shapeSizeZ / 2);
        float hectareX = -floor.transform.localScale.x/2 + (shapeSizeX / 2);

        for (int i = 0; i< 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject hectare = GameObject.CreatePrimitive(PrimitiveType.Cube);
                hectare.name = "Hectare";
                hectare.tag = "Hectare";
                hectare.transform.position = new Vector3(hectareX, 0, hectareZ);
                hectare.transform.localScale = new Vector3(shapeSizeX, 1, shapeSizeZ);


                hectareZ += shapeSizeZ;
            }
            hectareX += shapeSizeX;
            hectareZ = -floor.transform.localScale.z / 2 + (shapeSizeZ / 2);
        }

    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
