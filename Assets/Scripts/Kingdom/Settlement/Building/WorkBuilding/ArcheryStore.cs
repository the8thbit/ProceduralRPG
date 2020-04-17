using UnityEngine;
using UnityEditor;

public class ArcheryStore : WorkBuilding
{

    public static BuildingPlan BuildingPlan = new BuildingPlan("Archery Store", 15, 15);

    public ArcheryStore(int width, int height, int workCapacity = 2) : base(width, height)
    {
    }
}