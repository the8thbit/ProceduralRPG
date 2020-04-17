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
    public static BuildingPlan HOUSE = House.BuildingPlan;
    public static BuildingPlan MARKET = MarketPlace.BuildingPlan;
    public static BuildingPlan BARACKSCITY = Baracks.BuildingPlanCity;
    public static BuildingPlan BARACKSTOWN = Baracks.BuildingPlanTown;
    public static BuildingPlan ALCHEMISTS = Alchemists.BuildingPlan;
    public static BuildingPlan TAVERN = Tavern.BuildingPlan;
    public static BuildingPlan GENERALMERCHANT = GeneralMerchant.BuildingPlan;
    public static BuildingPlan SWORDSELLER = SwordSeller.BuildingPlan;
    public static BuildingPlan ARCHERYSTORE = ArcheryStore.BuildingPlan;
    public int SettlementID { get; private set; }
    public Vec2i SettlementCoord { get; private set; }
    public Tile[,] BuildingTiles { get; private set; }
    public WorldObjectData[,] BuildingObjects { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float BaseValue { get { return Width * Height; } }
    public float ValueModifier { get; private set; }
    public float Value { get { return BaseValue * ValueModifier; } }
    
    public Vec2i WorldPosition { get; private set; }

    private Recti WorldBounds;
    private List<Vec2i> SpawnableTiles;
    public List<BuildingEntrance> Entrances;


    public Vec2i Entrance { get; private set; }
    public Building(int width, int height)
    {
        Width = width;
        Height = height;

        BuildingTiles = new Tile[width, height];
        BuildingObjects = new WorldObjectData[width, height];
        Entrances = new List<BuildingEntrance>();
    }
    public void AfterApplyToWorld()
    {
        BuildingTiles = null;
        BuildingObjects = null;
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
        SettlementID = settle.SettlementID;
        if(this is WorkBuilding)
        {
            (this as WorkBuilding).WorldPositionSet();
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
    stone, wood,
}