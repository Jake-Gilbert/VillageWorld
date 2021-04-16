using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReproductionWithBias : MonoBehaviour
{
    private SortedList<AgentVillagerAdvanced.Personality, int> personalitiesToBeDistributed;
    private SortedList<AgentVillagerAdvanced.SpeedTrait, int> speedTraitsToBeDistributed;
    private SortedList<AgentVillagerAdvanced.StrengthTrait, int> strengthTraitsToBeDistributed;
    private int villagersToBeSpawned = 0;

    private T RouletteSelection<T>(KeyValuePair<T, float> lowerBound, KeyValuePair<T, float> upperBound)
    {
        float value = Random.value;
        if (value <= lowerBound.Value)
        {
            return lowerBound.Key;
        }
        //else if (value <= upperBound.Value)
        //{
        //    return upperBound.Key;
        //}
        return upperBound.Key;
    }

    private T RouletteSelection2<T>(List<T> bounds)
    {
        float value = Random.value;
        if (value <= 0.1)
        {
            return bounds[0];
        }
        else if (value <= 0.3)
        {
            return bounds[1];
        }
        return bounds[2];
    }


    private SortedList<T, int> TraitDistribution<T>(int numberOfVillagers, SortedList<T, int> traits)
    {
        SortedList<T, int> traitDistribution = new SortedList<T, int>();
        List<T> bounds = traits.Keys.ToList();
        int total = traits.Values.Sum();
        RouletteSelection(traits.Values.Min() / total  , traits.Values.Max() / total);

   
        //System.Enum.GetValues(typeof(T)).Cast<T>().ToList();
        foreach (T  trait in traits.Keys)
        {

        }
        
        //initalise list
        foreach (T item in bounds)
        {
            traitDistribution.Add(item, 0);
        }

        //distribute
        for (int i = 0; i < numberOfVillagers; i++)
        {
            traitDistribution[RouletteSelection2<T>(bounds)]++;
        }
        return traitDistribution;
    }

    //Calculates either personality, strength or speed trait of the population
    public void ProduceNewGeneration(int newVillagerAmount, SortedList<AgentVillagerAdvanced.Personality, int> p, SortedList<AgentVillagerAdvanced.SpeedTrait, int> sp, SortedList<AgentVillagerAdvanced.StrengthTrait, int> str)
    {//[selfish, 7], [neutral, 3];
        //TODO MNAYTBE CHANCE COLOUR
        int newVillagerTemp = newVillagerAmount;
        personalitiesToBeDistributed = TraitDistribution<AgentVillagerAdvanced.Personality>(5, p);
        speedTraitsToBeDistributed = new SortedList<AgentVillagerAdvanced.SpeedTrait, int>();
        strengthTraitsToBeDistributed = new SortedList<AgentVillagerAdvanced.StrengthTrait, int>();
        while (newVillagerTemp > 0)
        {

        }
    }
}
