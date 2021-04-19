using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ReproductionWithBiasT
{
    ReproductionWithBias reproductionWithBias = new ReproductionWithBias();
    int villagerAmount;
    SortedList<AgentVillagerAdvanced.Personality, int> personalityDistribution = new SortedList<AgentVillagerAdvanced.Personality, int>();
    SortedList<AgentVillagerAdvanced.SpeedTrait, int> speedDistribution = new SortedList<AgentVillagerAdvanced.SpeedTrait, int>();
    SortedList<AgentVillagerAdvanced.StrengthTrait, int> strengthDistribution = new SortedList<AgentVillagerAdvanced.StrengthTrait, int>();
   
  
    //[Test]
    //public void NewGenerationCorrectFunctionality()
    //{
    //    reproductionWithBias.ProduceNewGeneration(villagerAmount, personalityDistribution, speedDistribution, strengthDistribution);     
    //}
     
    [Test]
    public void DetermineHigherBoundPersonality()
    {
        personalityDistribution[AgentVillagerAdvanced.Personality.Empathetic] = 0;
        personalityDistribution[AgentVillagerAdvanced.Personality.Neutral] = 0;
        personalityDistribution[AgentVillagerAdvanced.Personality.Selfish] = 0;

        personalityDistribution[AgentVillagerAdvanced.Personality.Empathetic] += 5;
        personalityDistribution[AgentVillagerAdvanced.Personality.Neutral] += 2;
        personalityDistribution[AgentVillagerAdvanced.Personality.Selfish] += 10;

        KeyValuePair<AgentVillagerAdvanced.Personality, float> expected = new KeyValuePair<AgentVillagerAdvanced.Personality, float>(AgentVillagerAdvanced.Personality.Selfish, (float) 10/17);
        Assert.AreEqual( expected , reproductionWithBias.CalculateUpperBound(personalityDistribution));
    }
    [Test]
    public void DetermineMiddleBoundPersonality()
    {
        personalityDistribution[AgentVillagerAdvanced.Personality.Empathetic] = 0;
        personalityDistribution[AgentVillagerAdvanced.Personality.Neutral] = 0;
        personalityDistribution[AgentVillagerAdvanced.Personality.Selfish] = 0;

        personalityDistribution[AgentVillagerAdvanced.Personality.Empathetic] += 10;
        personalityDistribution[AgentVillagerAdvanced.Personality.Neutral] += 6;
        personalityDistribution[AgentVillagerAdvanced.Personality.Selfish] += 3;

        List<AgentVillagerAdvanced.Personality> upperAndLower = new List<AgentVillagerAdvanced.Personality>();
        upperAndLower.Add(personalityDistribution.Keys.ToList()[personalityDistribution.IndexOfValue(personalityDistribution.Values.Max())]);
        upperAndLower.Add(personalityDistribution.Keys.ToList()[personalityDistribution.IndexOfValue(personalityDistribution.Values.Min())]);

        AgentVillagerAdvanced.Personality middleBoundKey = personalityDistribution.Keys.Where(x => !upperAndLower.Contains(x)).LastOrDefault();
        int middleBound = personalityDistribution[middleBoundKey];
        KeyValuePair<AgentVillagerAdvanced.Personality, float> k = new KeyValuePair<AgentVillagerAdvanced.Personality, float>(middleBoundKey, (float) middleBound / personalityDistribution.Values.Sum());
        KeyValuePair<AgentVillagerAdvanced.Personality, float> expected = new KeyValuePair<AgentVillagerAdvanced.Personality, float>(AgentVillagerAdvanced.Personality.Neutral, (float) 6 / 19);
        Assert.AreEqual(expected, k);
    }
    [Test]
    public void DetermineLowerBoundPersonality()
    {
        personalityDistribution[AgentVillagerAdvanced.Personality.Empathetic] = 0;
        personalityDistribution[AgentVillagerAdvanced.Personality.Neutral] = 0;
        personalityDistribution[AgentVillagerAdvanced.Personality.Selfish] = 0;

        personalityDistribution[AgentVillagerAdvanced.Personality.Empathetic] += 7;
        personalityDistribution[AgentVillagerAdvanced.Personality.Neutral] += 0;
        personalityDistribution[AgentVillagerAdvanced.Personality.Selfish] += 2;

        KeyValuePair<AgentVillagerAdvanced.Personality, float> expected = new KeyValuePair<AgentVillagerAdvanced.Personality, float>(AgentVillagerAdvanced.Personality.Neutral, 0);
        Assert.AreEqual(expected, reproductionWithBias.CalculateLowerBound(personalityDistribution));
    }

}
