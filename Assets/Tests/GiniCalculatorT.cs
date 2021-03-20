using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GiniCalculatorT
{
    GiniCalculator giniCalculator = new GiniCalculator();
    [Test]
    public void ThreshholdsTenFruit()
    {
        float[] percentiles = new float[4] { 0.1F, 0.2F, 0.3F, 0.4F };
        int fruitCollected = 15;
        float[] expected = new float[4] { 2F, 3F, 5F, 6F };
        float[] thresholds = giniCalculator.CalculateThresholds(percentiles, fruitCollected);
        Assert.AreEqual(expected, thresholds);
    }

    [Test]
    public void QuantityOfWealth()
    {
        int[] wealthPerVillager = new int[6] { 2, 0, 0, 1, 3, 4 };
        float[] thresholds = new float[4] { 1, 2, 3, 4, };
        int[] quantityOfWealth = giniCalculator.CalculateQuantityOfWealth(thresholds, wealthPerVillager);
        int[] expected = new int[4] { 1, 2, 3, 4 };
        Assert.AreEqual(expected, quantityOfWealth);
    }

    [Test]
    public void PercentageOfWealth()
    {
        int[] wealthPerGroup = new int[4] { 7, 1, 3, 4 };
        int fruitCollected = 15;
        float[] percentages = giniCalculator.CalculateProportionOfWealth(wealthPerGroup, fruitCollected);
        float[] expected = new float[4] {(float) System.Math.Round(7F / 15F, 3), (float)System.Math.Round(1F / 15F, 3), (float)System.Math.Round(3F / 15F, 3), (float)System.Math.Round(4F / 15F, 3) };
        Assert.AreEqual(expected, percentages);
    }

    [Test]
    public void WealthDistribution1()
    {
        int[] divide = giniCalculator.CalculatePopulationDivide(new float[4] { 0, 1f, 3f, 5f }, new int[4] { 5, 0, 0, 3 });
        int[] expectedDivide = new int[4] { 2, 0, 1, 1 };
        Assert.AreEqual(expectedDivide, divide, "a");
        // Use the Assert class to test conditions
    }

    [Test]
    public void WealthDistributionAllZeroes()
    {
        int[] divide = giniCalculator.CalculatePopulationDivide(new float[4] { 0, 1f, 3f, 5f }, new int[6] { 0, 0, 0, 0, 0, 0 });
        int[] expectedDivide = new int[4] { 6, 0, 0, 0 };
        Assert.AreEqual(expectedDivide, divide, $"{string.Join(",", divide)}");
        // Use the Assert class to test conditions
    }
    

    [Test]
    public void WealthDistributionEvenSplit()
    {
        int[] divide = giniCalculator.CalculatePopulationDivide(new float[4] { 0, 1f, 3f, 5f }, new int[8] { 0, 0, 1, 1, 3, 3, 5, 5 });
        int[] expectedDivide = new int[4] { 2, 2, 2, 2 };
        Assert.AreEqual(expectedDivide, divide);
        // Use the Assert class to test conditions
    }



    [Test]
    public void GiniScoreHalf()
    {
        float[] scores = new float[4] { 0.05F, 0, 0.3F, 0.15F };
        float gini = giniCalculator.CalculateGiniCoefficient(scores);
        Assert.AreEqual(0.5F, gini);
    }
    [Test]
    public void GiniScoreZero()
    {
        float[] scores = new float[4] { 0.45F, 0.35F, 0.1F, 0.1F };
        float gini = giniCalculator.CalculateGiniCoefficient(scores);
        Assert.AreEqual(0F, gini);
    }

    [Test]
    public void GiniScoreOne()
    {
        float[] scores = new float[4] { 0, 0, 0, 0 };
        float gini = giniCalculator.CalculateGiniCoefficient(scores);
        Assert.AreEqual(1F, gini);
    }
}
