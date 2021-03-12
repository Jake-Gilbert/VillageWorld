using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    bool writtenToCsv = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (NoFruitBushes() && !writtenToCsv)
        {
            SaveToFile();
            writtenToCsv = true;
            
        }
    }

    string ToCSV()
    {
        int population = GameObject.FindGameObjectsWithTag("Villager").Length;
        GiniCalculator giniCalculator = GameObject.Find("GiniCalculator").GetComponent<GiniCalculator>();
        StringBuilder sb = new StringBuilder("Population,FruitCollected,Gini");
        foreach (float giniCoefficient in giniCalculator.giniValues)
        {
            int fruitCollected = GameObject.FindGameObjectWithTag("Zone").GetComponent<FloorZone>().GetFruitCount();

            sb.Append("\n").Append(population).Append(",").Append(fruitCollected).Append(",").Append(giniCoefficient);
        }
        return sb.ToString();
    }

    void SaveToFile()
    {
        string content = ToCSV();
        Debug.Log(content);
        var folder = Application.streamingAssetsPath;
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        var filePath = Path.Combine(folder, "gini.csv");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        File.WriteAllText(filePath, content);
        AssetDatabase.Refresh();
    }

    bool NoFruitBushes()
    {
        if (GameObject.FindGameObjectsWithTag("Bush").Length > 0)
        {
            return false;
        }
        else
        {
            return true;

        }
    }
}
