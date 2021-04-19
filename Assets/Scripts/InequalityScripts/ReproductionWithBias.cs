using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReproductionWithBias : MonoBehaviour
{
    private SortedList<AgentVillagerAdvanced.Personality, int> personalitiesToBeDistributed;
    private SortedList<AgentVillagerAdvanced.SpeedTrait, int> speedTraitsToBeDistributed;
    private SortedList<AgentVillagerAdvanced.StrengthTrait, int> strengthTraitsToBeDistributed;

    public KeyValuePair<T, float> CalculateUpperBound<T>(SortedList<T, int> listOfTraits)
    {
        int maxValue = listOfTraits.Values.Max();
        int total = listOfTraits.Values.Sum();
        return new KeyValuePair<T, float>(listOfTraits.Keys.ToList()[listOfTraits.IndexOfValue(maxValue)], (float)  maxValue / total);
    }

    public KeyValuePair<T, float> CalculateLowerBound<T>(SortedList<T, int> listOfTraits)
    {
        int minValue = listOfTraits.Values.Min();
        int total = listOfTraits.Values.Sum();
        return new KeyValuePair<T, float>(listOfTraits.Keys.ToList()[listOfTraits.IndexOfValue(minValue)], (float) minValue / total);
    }

    public SortedList<AgentVillagerAdvanced.Personality, int> getPersonalityDistribution()
    {
        return personalitiesToBeDistributed;
    }

    public SortedList<AgentVillagerAdvanced.SpeedTrait, int> getSpeedtDistribution()
    {
        return speedTraitsToBeDistributed;
    }

    public SortedList<AgentVillagerAdvanced.StrengthTrait, int> getStrengthDistribution()
    {
        return strengthTraitsToBeDistributed;
    }


    private T RouletteSelection< T>(KeyValuePair<T, float> lowerBound, KeyValuePair<T, float> middleBound, KeyValuePair<T, float> upperBound)
    {
        float value = Random.value;
        if (value <= lowerBound.Value)
        {
            return lowerBound.Key;
        }
        else if (value <= middleBound.Value)
        {
            return middleBound.Key;
        }
        return upperBound.Key;
    }

    private SortedList<T, int> TraitDistribution<T>(int numberOfVillagers, SortedList<T, int> traits)
    {
        SortedList<T, int> traitDistribution = new SortedList<T, int>();       
        int total = traits.Values.Sum();

        KeyValuePair<T, float> lowerBoundPair = CalculateLowerBound(traits);
        KeyValuePair<T, float> upperBoundPair = CalculateUpperBound(traits);

        List<T> upperAndLower = new List<T>();
        upperAndLower.Add(upperBoundPair.Key);
        upperAndLower.Add(lowerBoundPair.Key);

        T middleBoundKey = traits.Keys.Where(x => !upperAndLower.Contains(x)).LastOrDefault();
        KeyValuePair<T, float> middleBoundPair = new KeyValuePair<T, float>(middleBoundKey, traits[middleBoundKey] / total);
        int villagersToSpawn = numberOfVillagers;
        while (villagersToSpawn > 0)
        {
            T traitToAdd = RouletteSelection(lowerBoundPair, middleBoundPair, upperBoundPair);
            if (!traitDistribution.Keys.Contains(traitToAdd))
            {
                traitDistribution.Add(traitToAdd, 1);
            }
            else
            {
                traitDistribution[traitToAdd] += 1;
            }
            villagersToSpawn--;
        }
        return traitDistribution;
    }

    //Calculates either personality, strength or speed trait of the population
    public void ProduceNewGeneration(int newVillagerAmount, SortedList<AgentVillagerAdvanced.Personality, int> p, SortedList<AgentVillagerAdvanced.SpeedTrait, int> sp, SortedList<AgentVillagerAdvanced.StrengthTrait, int> str)
    {
        int newVillagerTemp = newVillagerAmount;
        personalitiesToBeDistributed = TraitDistribution(newVillagerAmount, p);
        speedTraitsToBeDistributed = TraitDistribution(newVillagerAmount, sp);
        strengthTraitsToBeDistributed = TraitDistribution(newVillagerTemp, str);
    }
}
