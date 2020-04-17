using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
/// <summary>
/// A chunk structure is a structure that exists outside of settlements.
/// it is defined by its base position (minX, minZ) and its size (sizeX,sizeZ) in chunks.
/// 
/// </summary>
[System.Serializable]
public abstract class ChunkStructure
{

    public WorldMapLocation WorldMapLocation { get; private set; }
    public string Name { get; private set; }
    public Vec2i Position { get; private set; }
    public Vec2i Size { get; private set; }

    public List<Entity>[,] StructureEntities;
    private List<Entity> AllEntities;
    public DungeonEntrance DungeonEntrance { get; protected set; }
    public bool HasDungeonEntrance { get { return DungeonEntrance != null; } }
    public int ID { get; private set; }


    public IInventoryObject FinalLootChest { get; private set; }

    public ChunkStructure(Vec2i position, Vec2i size, string name = "DefChunkStructure")
    {
        Position = position;
        Size = size;
        Name = name;
        StructureEntities = new List<Entity>[size.x, size.z];
        AllEntities = new List<Entity>();
    }

    public void SetWorldMapLocation(WorldMapLocation wml)
    {
        WorldMapLocation = wml;
    }

    public void AddEntity(int localX, int localZ, Entity e)
    {
        if (StructureEntities[localX, localZ] == null)
            StructureEntities[localX, localZ] = new List<Entity>();
        StructureEntities[localX, localZ].Add(e);
        AllEntities.Add(e);
    }


    public Entity ChooseRandomEntity(GenerationRandom ran = null)
    {
        Debug.Log(AllEntities.Count);
        if(ran == null)
            return GameManager.RNG.RandomFromList(AllEntities);
        return ran.RandomFromList(AllEntities);
    }

    public void SetDungeonEntrance(DungeonEntrance dunEntrance)
    {
        DungeonEntrance = dunEntrance;
    }

    public void SetLootChest(IInventoryObject invOb)
    {
        FinalLootChest = invOb;
    }

    public void SetID(int id)
    {
        ID = id;
    }

}