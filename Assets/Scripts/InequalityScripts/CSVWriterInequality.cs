using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CSVWriterInequality : CSVWriter
{
    private bool writtenToCsv = false;
    [SerializeField]
    private GiniCalculatorInequality giniCalculator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!writtenToCsv && !FindObjectOfType<GiniCalculatorInequality>().running)
        {
            writtenToCsv = true;
            SaveToFile();
        }
    }

    protected new string ToCSV()
    {
        int villagersAlive = GameObject.FindGameObjectsWithTag("Villager").Length;
        StringBuilder sb = new StringBuilder("Generation,Max Population, Final Population," +
            "TotalFruitCollected, FruitAvailable, MinFruitCollected,MaxFruitColllected,AvgFruitCollected, Dominant Personality, Dominant Speed," +
            "Dominant Strength, Quantity of Personality, Quantity of Speed, Quantity of Strength ,Gini");
        foreach (string csvInfo in giniCalculator.inequalityValues)
        {
            string[] values = csvInfo.Split(',');
            Debug.Log(string.Join(",", values));
            sb.Append("\n").Append(values[0]).Append(",").Append(values[1]).Append(",").Append(values[2]).Append(",").Append(values[3]).Append(",");
            sb.Append(values[4]).Append(",").Append(values[5]).Append(",").Append(values[6]).Append(",").Append(values[7]).Append(",").Append(values[8]);
            sb.Append(",").Append(values[9]).Append(",").Append(values[10]).Append(",").Append(values[11]).Append(",").Append(values[12]).Append(",");
            sb.Append(values[13]).Append(",").Append(values[14]).Append(",").Append(values[15]);
        }
        return sb.ToString();
    }

    protected new void SaveToFile()
    {
        string content = ToCSV();
        Debug.Log(content);
        var folder = Application.streamingAssetsPath;
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        var filePath = Path.Combine(folder, "giniInequality.csv");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        File.WriteAllText(filePath, content);
        AssetDatabase.Refresh();
    }


}
