using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AgentT
{
    AgentVillagerAdvanced agent = new AgentVillagerAdvanced();
    // A Test behaves as an ordinary method
    [Test]
    public void Pick0Fruit()
    {
        agent.currentHeldFruit = 0;
        Assert.AreEqual(0, agent.currentHeldFruit);
        // Use the Assert class to test conditions
    }
  
}
