using UnityEngine;
using UnityEditor;

public class SwordSeller : WorkBuilding
{
    public static BuildingPlan BuildingPlan = new BuildingPlan("Sword Seller", 15, 15);
    public SwordSeller(int width, int height) : base(width, height)
    {
    }
}