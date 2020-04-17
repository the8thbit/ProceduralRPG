using UnityEngine;
using UnityEditor;

public class Hold : House
{
    public static BuildingPlan BuildingPlan = new BuildingPlan("Hold", 16, 24);
    public Hold(int width, int height) : base(width, height)
    {
    }
}