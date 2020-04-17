using UnityEngine;
using UnityEditor;
[System.Serializable]
public class Tavern : WorkBuilding
{
    public static BuildingPlan BuildingPlan = new BuildingPlan("Tavern", 20, 30);
    public Tavern(int width, int height) : base(width, height)
    {
    }
}