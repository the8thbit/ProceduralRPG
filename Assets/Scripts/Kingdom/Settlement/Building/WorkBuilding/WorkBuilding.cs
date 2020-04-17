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
[System.Serializable]
public abstract class WorkBuilding : Building
{



    public int WorkCapacity { get { return AllJobs.Count; } }
    public Inventory WorkStore { get; private set; } //The storage of items the work building can buy/sell TODO
    public List<WorkEquiptmentPlacement> WorkEquiptment { get; private set; }
    private List<NPCJob> AllJobs;
    public WorkBuilding(int width, int height) : base(width, height)
    {
        WorkEquiptment = new List<WorkEquiptmentPlacement>();
        AllJobs = new List<NPCJob>();

    }



    public void AddJob(NPCJob job)
    {
        AllJobs.Add(job);
    }

  

    public List<NPCJob> GetJobs()
    {
        return AllJobs;
    }
    public void WorldPositionSet()
    {
        foreach(WorkEquiptmentPlacement wep in WorkEquiptment)
        {
            wep.SetWorldPosition(this.WorldPosition + wep.LocalPositon);
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (!(obj is WorkBuilding))
            return false;

        return (obj as Building).Equals(this);
    }
    public override string ToString()
    {
        return "WorkBuilding " + this.WorldPosition;
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