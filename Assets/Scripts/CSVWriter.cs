using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    private bool writtenToCsv = false;

    // Update is called once per frame
    private void Update()
    {
        if ((NoFruitBushes() || NoVillagers()) && !writtenToCsv)
        {
            SaveToFile();
            writtenToCsv = true;
        }
    }

    private string ToCSV()
    {
        int villagersAlive = GameObject.FindGameObjectsWithTag("Villager").Length;
        GiniCalculator giniCalculator = GameObject.Find("GiniCalculator").GetComponent<GiniCalculator>();
        StringBuilder sb = new StringBuilder("VillagersAlive,MinFruitCollected,MaxFruitColllected,AvgFruitCollected,TotalFruitCollected,Gini");
        foreach (string csvInfo in giniCalculator.giniValues)
        {
            string[] values = csvInfo.Split(',');
            Debug.Log(string.Join(",",values));
            sb.Append("\n").Append(values[0]).Append(",").Append(values[1]).Append(",").Append(values[2]).Append(",").Append(values[3]).Append(",").Append(values[4]).Append(",").Append(values[5]);
        }
        return sb.ToString();
    }

    private void SaveToFile()
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

    private bool NoFruitBushes()
    {
        return GameObject.FindGameObjectsWithTag("Bush").Length <= 0;
    }

    private bool NoVillagers()
    {
        return GameObject.FindGameObjectsWithTag("Villager").Length <= 0;
    }
}
