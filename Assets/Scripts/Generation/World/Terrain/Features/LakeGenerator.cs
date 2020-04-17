using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
/// <summary>
/// Lake generator defines all lakes in the game
/// </summary>
public class LakeGenerator
{


    private List<Lake> Lakes;
    private GameGenerator GameGenerator;
    public LakeGenerator(GameGenerator gamGen)
    {
        GameGenerator = gamGen;
        Lakes = new List<Lake>();
    }

    public void AddLake(Lake lake)
    {

        Lakes.Add(lake);
    }



    /// <summary>
    /// Iterates all lakes in <see cref="LakeGenerator.Lakes"/>
    /// and adds the lake to all chunks within the radius
    /// </summary>
    public void GenerateAllLakes()
    {
        foreach(Lake lake in Lakes)
        {
            Vec2i mid = lake.Centre;
            int rad = (int)lake.Radius;
            for(int x=-rad; x<= rad; x++)
            {
                for (int z = -rad; z <= rad; z++)
                {
                    int xClamp = Mathf.Clamp(mid.x + x, 0, World.WorldSize-1);
                    int zClamp = Mathf.Clamp(mid.z + z, 0, World.WorldSize-1);
                    GameGenerator.TerrainGenerator.ChunkBases[xClamp, zClamp].AddLake(lake);
                }
            }
        }
    }

}