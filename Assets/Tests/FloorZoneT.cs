using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FloorZoneT
{
    FloorZoneAdvanced floorZone = new FloorZoneAdvanced();
    // A Test behaves as an ordinary method
    [Test]
    public void PlaceFruit0()
    {
        floorZone.PlaceFruit(0);
        Assert.AreEqual(0, floorZone.GetFruitCount());
    }


}
