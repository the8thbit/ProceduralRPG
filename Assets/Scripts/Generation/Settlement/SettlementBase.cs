using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
public class SettlementBase
{
    public Vec2i BaseCoord { get; private set; }
    //Defines the middle tile of the Settelment in local coordinates
    private Vec2i MidTile { get; }
    //The chunk coordinate at the centre of the settlement, defines its position in the world
    public Vec2i Centre { get; private set; }
    public Vec2i BaseChunk { get; private set; }
    //An array of all the chunks that this settelment contains/belongs to.
    public Vec2i[] SettlementChunks { get; private set; }
    public int TileSize { get; private set; }
    public int ChunkSize { get; private set; }
    public SettlementType SettlementType { get; private set; }
    public SettlementBase(Vec2i centreChunk, int size, SettlementType type)
    {
        Centre = centreChunk;
        BaseCoord = (Centre - new Vec2i(size, size)) * World.ChunkSize; //The coordinate of the minimum (minX,minZ) point of the settlement
        SettlementType = type;
        BaseChunk = new Vec2i(Centre.x - size, Centre.z - size);

        List<Vec2i> settlementChunks = new List<Vec2i>();

        //Itterate all chunks within bounds, add them to the list
        for (int x = -size; x <= size; x++)
        {
            for (int z = -size; z <= size; z++)
            {
                settlementChunks.Add(new Vec2i(Centre.x + x, Centre.z + z));
            }
        }
        SettlementChunks = settlementChunks.ToArray(); //Store the list to a local array


        TileSize = (size * 2 + 1) * World.ChunkSize;
        ChunkSize = TileSize / World.ChunkSize;
    }
}