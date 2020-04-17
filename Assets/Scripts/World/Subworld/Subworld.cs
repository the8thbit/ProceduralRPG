using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
[System.Serializable]
public class Subworld
{


    public ChunkData[,] SubworldChunks { get; protected set; }
    public Vec2i SubworldEntrance { get; protected set; } //Where in the world the dungeon entrance/exit is
    public Vec2i WorldEntrance { get; protected set; } //Where in the local dungeon space the player goes when entering

    public Vec2i ChunkSize { get; private set; }
    public int SubworldID { get; private set; }


    public Subworld(ChunkData[,] subChunks, Vec2i subentra, Vec2i worldEntrance)
    {
        SubworldChunks = subChunks;
        SubworldEntrance = subentra;
        WorldEntrance = worldEntrance;
        ChunkSize = new Vec2i(subChunks.GetLength(0), subChunks.GetLength(1));


    }
    public void SetSubworldID(int id)
    {
        SubworldID = id;
    }

    public void SetWorldEntrance(Vec2i ent)
    {
        WorldEntrance = ent;
    }

    public ChunkData GetChunkSafe(int x, int z)
    {
        if (SubworldChunks.GetLength(0) <= x || x < 0)
            return null;
        if (SubworldChunks.GetLength(1) <= z || z < 0)
            return null;
        return SubworldChunks[x, z];
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        Subworld ot = obj as Subworld;
        if (ot == null)
            return false;
        return ot.SubworldID == SubworldID;
    }
}