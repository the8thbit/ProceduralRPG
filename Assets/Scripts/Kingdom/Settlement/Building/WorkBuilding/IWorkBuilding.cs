using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// If a work building is a store, then it can sell items.
/// </summary>
public interface IStore
{/*
    public abstract Inventory GetSellable();
    public void RefreshStock();
    public List<NPC> GetShopMerchants();
    */
}
public interface IWorkBuilding
{

    /// <summary>
    /// Each work building must hold onto a WorkBuildingData struct
    /// </summary>
    WorkBuildingData GetWorkData { get; }

    void SetWorkBuildingData(WorkBuildingData data);
    Building WorkBuilding { get; }

}

/// <summary>
/// Contains details related to a workbuilding
/// </summary>
[System.Serializable]
public struct WorkBuildingData
{
    
    public NPCJob[] BuildingJobs { get; private set; }
    public int WorkCapacity { get { return BuildingJobs.Length; } }
    public Inventory WorkInventory { get; private set; }

    public WorkBuildingData(NPCJob[] buildingJobs)
    {
        BuildingJobs = buildingJobs;
        WorkInventory = new Inventory();
    }

}

[System.Serializable]
public class WorkEquiptmentPlacement
{
    public WorkEquiptmentData Equiptment { get; private set; }
    public WorkEquiptmentData Equiptment_ { get { return (WorkEquiptmentData)GameManager.WorldManager.World.GetWorldObject(WorldPosition); } }
    public Vec2i LocalPositon { get; private set; }
    public Vec2i WorldPosition { get; private set; }

    public Entity User { get; private set; }

    public WorkEquiptmentPlacement(WorkEquiptmentData equipt, Vec2i localPosition)
    {
        Equiptment = equipt;
        LocalPositon = localPosition;

    }
    public void SetWorldPosition(Vec2i wPos)
    {
        WorldPosition = wPos;
    }
    public void SetUser(Entity entity)
    {
        User = entity;
    }
}