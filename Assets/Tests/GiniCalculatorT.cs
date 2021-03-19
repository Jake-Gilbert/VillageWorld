using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestController 
{
    
    // A Test behaves as an ordinary method
    [Test]
    public void WealthDistribution1()
    {
        GiniCalculator giniCalculator = new GiniCalculator();
        int[] divide = giniCalculator.CalculatePopulationDivide(new float[4] { 0, 1f, 3f, 5f }, new int[4] {5,0, 0, 3});
        int[] expectedDivide = new int[4] {2, 0,1,1};
        Assert.AreEqual(expectedDivide, divide, "a");      
        // Use the Assert class to test conditions
    }

    [Test]
    public void WealthDistributionAllZeroes()
    {
        GiniCalculator giniCalculator = new GiniCalculator();
        int[] divide = giniCalculator.CalculatePopulationDivide(new float[4] { 0, 1f, 3f, 5f }, new int[6] { 0, 0, 0, 0, 0, 0 });
        int[] expectedDivide = new int[4] {6, 0, 0, 0 };
        Assert.AreEqual(expectedDivide, divide, $"{string.Join(",", divide)}" );
        // Use the Assert class to test conditions
    }

    [Test]
    public void WealthDistributionEvenSplit()
    {
        GiniCalculator giniCalculator = new GiniCalculator();
        int[] divide = giniCalculator.CalculatePopulationDivide(new float[4] { 0, 1f, 3f, 5f }, new int[8] { 0, 0, 1, 1, 3, 3, 5, 5 });
        int[] expectedDivide = new int[4] { 2, 2, 2, 2 };
        Assert.AreEqual(expectedDivide, divide);
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestControllerWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
