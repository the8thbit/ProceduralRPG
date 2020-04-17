using UnityEngine;
using UnityEditor;
[System.Serializable]
public class MarketPlace : WorkBuilding
{

    public static BuildingPlan BuildingPlan = new BuildingPlan("Market", 20, 32);

    public MarketPlace(int width, int height) : base(width, height)
    {
    }
}