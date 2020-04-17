using UnityEngine;
using UnityEditor;
[System.Serializable]
public class Blacksmith : WorkBuilding
{
    public static BuildingPlan BuildingPlan = new BuildingPlan("Blacksmith", 15, 25);
    public Blacksmith(int width, int height) : base(width, height)
    {
    }
}