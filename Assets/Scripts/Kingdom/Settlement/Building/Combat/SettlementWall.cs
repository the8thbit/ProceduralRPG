using UnityEngine;
using UnityEditor;
[System.Serializable]
public class SettlementWall : Building
{

    public static BuildingPlan BuildingPlan = new BuildingPlan("Barracks", 25, 35);
    public SettlementWall(int width, int height) : base(width, height)
    {
    }

    
}