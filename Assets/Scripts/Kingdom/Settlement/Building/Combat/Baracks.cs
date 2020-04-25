using UnityEngine;
using UnityEditor;
[System.Serializable]
public class Baracks : Building, IWorkBuilding
{
    public static BuildingPlan BuildingPlanCity = new BuildingPlan("Barracks", 25, 35);
    public static BuildingPlan BuildingPlanTown = new BuildingPlan("Barracks", 15, 25);
    public Baracks(int width, int height) : base(width, height)
    {
    }

    public WorkBuildingData GetWorkData => throw new System.NotImplementedException();

    public void SetWorkBuildingData(WorkBuildingData data)
    {
        throw new System.NotImplementedException();
    }
}