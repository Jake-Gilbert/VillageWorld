using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CSVWriterEvolution : MonoBehaviour
{
    private bool writtenToCsv = false;
    private VillagerStatsEvolution villagerStats;
    private bool initialised = false;
    private GenerationBehavioursEvolution generationBehaviours;


    // Update is called once per frame
    void Update()
    {
        if (!initialised)
        {
            generationBehaviours = FindObjectOfType<GenerationBehavioursEvolution>();
            villagerStats = FindObjectOfType<VillagerStatsEvolution>();
            initialised = true; ;
        }
        if (!writtenToCsv && villagerStats.GetGeneration() > 30)
        {
            writtenToCsv = true;
            SaveToFile();
        }
        if (GameObject.FindGameObjectsWithTag("Villager").Length <= 0)
        {
            writtenToCsv = true;
            SaveToFile();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            writtenToCsv = true;
            SaveToFile();
        }
    }

    public string ToCSV()
    {
        int villagersAlive = GameObject.FindGameObjectsWithTag("Villager").Length;
        StringBuilder sb = new StringBuilder("Generation, TotalFruitCollected, FruitCollectedGeneration, MeanDesireToReveal," +
           "MeanDesireToShare, MeanBaseEnergyLossRate, MeanEnergy, MeanSpeed, MeanCarryingCapacity, HighestFitness");
        foreach (string csvInfo in generationBehaviours.evolutionValues)
        {
            string[] values = csvInfo.Split(',');
            sb.Append("\n").Append(values[0]).Append(",").Append(values[1]).Append(",").Append(values[2]).Append(",").Append(values[3]).Append(",");
            sb.Append(values[4]).Append(",").Append(values[5]).Append(",").Append(values[6]).Append(",").Append(values[7]).Append(",").Append(values[8]);
            sb.Append(",").Append(values[9]);
        }
        return sb.ToString();
    }

    protected void SaveToFile()
    {
        string content = ToCSV();
        var folder = Application.streamingAssetsPath;
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        var filePath = Path.Combine(folder, "giniEvolution.csv");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        File.WriteAllText(filePath, content);
        AssetDatabase.Refresh();
    }

}

