using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Crossover : MonoBehaviour
{
    public List<List<float>> offspring;
    public List<float> offspringTraits;
    public List<float> offspringTwoTraits;
    public Crossover(AgentVillagerEvolution parentOne, AgentVillagerEvolution parentTwo)
    {
        offspring = new List<List<float>>();
        int[] parentOneBits = new int[6];
        int[] parentTwoBits = new int[6];

        for (int i = 0; i < 6; i++)
        {
            parentTwoBits[i] = 1;
        }
        int start = Random.Range(0, parentOneBits.Length - 2);
        int end = Random.Range(start + 1, parentOneBits.Length - 1);
        int[] offspringOneBits = FillUsingSplice(6, start, end);
        int[] offspringTwoBits = ReverseOffSpringBits(offspringOneBits);

        offspringOneBits = TryMutation(offspringOneBits);
        offspringTwoBits = TryMutation(offspringTwoBits);

        offspringTraits = ProduceOffspring(offspringOneBits, parentOne, parentTwo);
        offspringTwoTraits = ProduceOffspring(offspringTwoBits, parentOne, parentTwo);
        offspring.Add(offspringTraits);
        offspring.Add(offspringTwoTraits);
    }
    int[] FillUsingSplice(int arraySize, int startPoint, int endPoint)
    {
        int[] result = new int[arraySize];
        for (int i = startPoint; i <= endPoint; i++)
        {
            result[i] = 1;
        }
        return result;
    }

    private List<float> ProduceOffspring(int[] offspringBits, AgentVillagerEvolution parentOne, AgentVillagerEvolution parentTwo)
    {
        List<float> traits = new List<float>();
        for (int i = 0; i < offspringBits.Length; i++)
        {
            traits.Add(offspringBits[i] < 1 ? parentOne.traits[i] : parentTwo.traits[i]);
        }
        return traits;
    }
    private int[] ReverseOffSpringBits(int[] offspringToReverse)
    {
        return offspringToReverse.Select(x => x == 0 ? 1 : 0).ToArray();
    }

    private int[] TryMutation(int[] offspringBits)
    {
        float mutationChance = 0.02F;
        for (int i = 0; i < offspringBits.Length; i++)
        {
            if (Random.value <= mutationChance)
            {
                offspringBits[i] = offspringBits[i] == 0 ? 1 : 0;
            }
        }
        return offspringBits;
    }
}
