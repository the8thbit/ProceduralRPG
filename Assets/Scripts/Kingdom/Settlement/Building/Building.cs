using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildingPlan
{
    public string Name { get; private set; }
    public int MinSize { get; private set; }
    public int MaxSize { get; private set; }
    public BuildingPlan(string buildingName, int minSize, int maxSize)
    {
        Name = buildingName;
        MinSize = minSize;
        MaxSize = maxSize;
    }
}
[System.Serializable]
public abstract class Building
{

    public static BuildingPlan VILLAGEHALL = VillageHall.BuildingPlan;
    public static BuildingPlan HOLD = Hold.BuildingPlan;
    public static BuildingPlan CITYCASTLE = Castle.BuildingPlanSmall;
    public static BuildingPlan CAPTIALCASTLE = Castle.BuildingPlanBig;

    public static BuildingPlan BLACKSMITH = Blacksmith.BuildingPlan;
    public static BuildingPlan MARKET = MarketPlace.BuildingPlan;
    public static BuildingPlan HOUSE = House.BuildingPlan;
    public static BuildingPlan BARACKS = Baracks.BuildingPlanCity;
    public int SettlementID { get; private set; }
    public Vec2i SettlementCoord { get; private set; }
    public Tile[,] BuildingTiles { get; private set; }
    public WorldObjectData[,] BuildingObjects { get; private set; }
    private List<WorldObjectData> BuildingObjects_; 
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float BaseValue { get { return Width * Height; } }
    public float ValueModifier { get; private set; }
    public float Value { get { return BaseValue * ValueModifier; } }
    
    public Vec2i WorldPosition { get; private set; }

    private Recti WorldBounds;
    private List<Vec2i> SpawnableTiles;


    public Vec2i Entrance { get; private set; }
    public Building(int width, int height)
    {
        Width = width;
        Height = height;

        BuildingTiles = new Tile[width, height];
        BuildingObjects = new WorldObjectData[width, height];
        BuildingObjects_ = new List<WorldObjectData>();
    }
    /// <summary>
    /// Called after the building has been added to the world.
    /// Clears all objects in building from memory, as they are now stored
    /// in ChunkData 
    /// </summary>
    public void AfterApplyToWorld()
    {
        BuildingTiles = null;
        BuildingObjects = null;
    }

    public List<WorldObjectData> GetBuildingObjects()
    {
        return BuildingObjects_;
    }

    /// <summary>
    /// Adds the specified WorldObject to the list containing
    /// all objects in this building.
    /// WARNING - this does not add the object to the array of objects itself
    /// </summary>
    public void AddObjectReference(WorldObjectData obj) {
        BuildingObjects_.Add(obj);
    }

    public void SetValueModifier(float v)
    {
        ValueModifier = v;
    }
    public void SetEntrancePoint(Vec2i v)
    {
        Entrance = v;
    }

    public void SetBuilding(Tile[,] buildingTiles, WorldObjectData[,] buildingObjects)
    {
        BuildingTiles = buildingTiles;
        BuildingObjects = buildingObjects;
    }

    public void SetSettlementCoord(Vec2i setCoord)
    {
        SettlementCoord = setCoord;
    }
    public void SetSettlement(Settlement settle)
    {
        WorldPosition = settle.BaseCoord + this.SettlementCoord;
        if(Entrance != null)
            Entrance += settle.BaseCoord + this.SettlementCoord;
        SettlementID = settle.SettlementID;

        Vec2i delta = settle.BaseCoord + SettlementCoord;
        foreach(WorldObjectData obj in BuildingObjects)
        {
            if(obj != null)
            {
                obj.SetPosition(obj.WorldPosition + delta);
            }
        }
        
    }



    public Recti GetWorldBounds()
    {
        if(WorldBounds == null)
        {
            WorldBounds = new Recti(WorldPosition.x, WorldPosition.z, Width, Height);
        }

        return WorldBounds;
    }

    public List<Vec2i> GetSpawnableTiles(bool force=false)
    {
        if (SpawnableTiles == null || force)
        {

            SpawnableTiles = new List<Vec2i>();
            Vec2i worldPos = WorldPosition;
            for (int x=0; x<Width; x++)
            {
                for(int z=0; z<Height; z++)
                {
                    if(BuildingObjects[x,z] == null)
                    {
                        SpawnableTiles.Add(new Vec2i(worldPos.x + x, worldPos.z + z));
                    }
                }
            }
        }

        return SpawnableTiles;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (!(obj is Building))
            return false;
        return WorldPosition.Equals((obj as Building).WorldPosition);
    }
}

public enum BuildingStyle
{
    stone, wood, brick, sandstone
}