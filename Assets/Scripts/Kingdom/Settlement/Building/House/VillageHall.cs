using UnityEngine;
using UnityEditor;

public class VillageHall : House
{
    public static BuildingPlan BuildingPlan = new BuildingPlan("Village Hall", 8, 15);
    public VillageHall(int width, int height) : base(width, height)
    {
    }
}