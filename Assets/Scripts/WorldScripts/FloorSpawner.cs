using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    public int sizeX;
    public int sizeZ;

    public int numOfCols;
    public int numOfRows;

    private void Start()
    {
        GameObject floor = (GameObject)Instantiate(Resources.Load("Floor"), new Vector3(-sizeX/2, 0, -sizeX/2), Quaternion.identity);
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
        floor.GetComponent<Terrain>().terrainData.size = new Vector3(sizeX, 0, sizeZ); 
        floor.name = "Floor";
        floor.tag = "Boundary";

        //Four boundary walls
        BoxCollider north = floor.AddComponent(typeof(BoxCollider)) as BoxCollider;
        north.center = new Vector3(sizeX / 2, 0, sizeZ);
        north.size = new Vector3(sizeX, 50, 0);

        BoxCollider east = floor.AddComponent(typeof(BoxCollider)) as BoxCollider;
        east.center = new Vector3(sizeX, 0, sizeZ/2);
        east.size = new Vector3(0, 50, sizeX);

        BoxCollider south = floor.AddComponent(typeof(BoxCollider)) as BoxCollider;
        south.center = new Vector3(sizeX/2, 0, 0);
        south.size = new Vector3(sizeX, 50, 0);


        BoxCollider west = floor.AddComponent(typeof(BoxCollider)) as BoxCollider;
        west.center = new Vector3(0, 0, sizeZ/2);
        west.size = new Vector3(0, 50, sizeZ);

       

        float floorX = gameObject.transform.localPosition.x;
        float floorZ = gameObject.transform.localPosition.z;
        GameObject floorzone = (GameObject)Instantiate(Resources.Load("CollectionZone"), new Vector3(Random.Range(-floorX, -floorZ), 0F, Random.Range(-floorZ, floorZ)), Quaternion.identity);
        floorzone.tag = ("Zone");
        FloorZone floorzoneScript = floorzone.GetComponent(typeof(FloorZone)) as FloorZone;
        floorzoneScript.floor = floor;

        float shapeSizeZ = sizeZ / numOfCols;
        float shapeSizeX = sizeX / numOfRows;

        float hectareZ = sizeZ /2 - (shapeSizeZ/2);
        float hectareX = sizeX  /2 - (shapeSizeX/2);

        for (int i = 0; i< numOfCols; i++)
        {
            for (int j = 0; j < numOfRows; j++)
            {
                GameObject hectare = GameObject.CreatePrimitive(PrimitiveType.Cube);
                hectare.name = "Hectare";
                hectare.tag = "Hectare";
                hectare.GetComponent<MeshRenderer>().enabled = false;
                hectare.transform.position = new Vector3(hectareX, 0, hectareZ);
                hectare.transform.localScale = new Vector3(shapeSizeX, 1, shapeSizeZ);


                hectareZ -= shapeSizeZ;
            }
            hectareX -= shapeSizeX;
            hectareZ = sizeZ / 2 - (shapeSizeZ / 2);
        }
        

    }

  
}
