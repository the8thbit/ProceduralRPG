using UnityEngine;
using UnityEditor;

public class MarketPlace : Building, IWorkBuilding
{
    public static BuildingPlan BuildingPlan = new BuildingPlan("Market Place", 15, 25);
    private WorkBuildingData WorkBuildingData;
    public MarketPlace(int width, int height) : base(width, height)
    {
    }

    public WorkBuildingData GetWorkData => WorkBuildingData;

    public Building WorkBuilding => this;

    public void SetWorkBuildingData(WorkBuildingData data)
    {
        WorkBuildingData = data;
    }
}