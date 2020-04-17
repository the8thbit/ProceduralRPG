using UnityEngine;
using UnityEditor;
[System.Serializable]
public class Alchemists : WorkBuilding
{
    public static BuildingPlan BuildingPlan = new BuildingPlan("Alchemists", 10, 15);

    public Alchemists(int width, int height) : base(width, height)
    {
    }
}