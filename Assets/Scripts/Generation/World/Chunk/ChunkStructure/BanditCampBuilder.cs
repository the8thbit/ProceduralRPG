using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
public class BanditCampBuilder
{


    private Vec2i TileSize;
    private int[,] Tiles;
    private WorldObjectData[,] Objects;
    private BanditCamp Shell;
    public IInventoryObject FinalLootChest { get; private set; }
    

    public BanditCampBuilder(BanditCamp shell)
    {
        Shell = shell;
        TileSize = shell.Size * World.ChunkSize;
        

        Tiles = new int[TileSize.x, TileSize.z];
        Objects = new WorldObjectData[TileSize.x, TileSize.z];
    }


    public List<ChunkData> Generate(GenerationRandom genRan)
    {



        Vec2i tilebase = Shell.Position * World.ChunkSize;

        if (Shell.BanditCampLevel > 1 && Shell.Size.x>3 && Shell.Size.z > 3)
        {
            //If this camp is large enough, generate a dungeon entrance.

            Vec2i localPos = new Vec2i(2, TileSize.z / 2 - 2);

            CaveDungeonEntrance entr = new CaveDungeonEntrance(tilebase + localPos, null, new WorldObjectMetaData(direction: new Vec2i(1, 0)));
            IMultiTileObjectChild[,] children = entr.GetChildren();
            Objects[localPos.x, localPos.z] = entr;
            for(int x=0; x<entr.Size.x; x++)
            {
                for (int z = 0; z < entr.Size.z; z++)
                {
                    if (x == 0 && z == 0)
                        continue;
                    Objects[localPos.x + x, localPos.z + z] = children[x, z] as WorldObjectData;
                }
            }
            Debug.Log("Generated Bandit Camp with Dungeon at " + this.Shell.Position, Debug.CHUNK_STRUCTURE_GENERATION);
            
            Shell.SetDungeonEntrance(entr);
            entr.SetChunkStructure(Shell);
        }
        else
        {
            Debug.Log("Generated Bandit Camp no Dungeon at " + this.Shell.Position, Debug.CHUNK_STRUCTURE_GENERATION);

        }

        Objects[11, 11] = new LootSack(tilebase + new Vec2i(11, 11));
        FinalLootChest = Objects[11, 11] as IInventoryObject;

        for (int x=0; x<TileSize.x; x++)
        {
            for (int z = 0; z < TileSize.z; z++)
            {

                if (x == 0)
                {
                    Objects[x, z] = new WoodSpikeWall(tilebase + new Vec2i(x, z));
                }
                if(z == 0)
                {
                    Objects[x, z] = new WoodSpikeWall(tilebase + new Vec2i(x, z));
                }
                if(x==TileSize.x-1 && z<TileSize.z/2-2 && z > TileSize.z / 2 + 2)
                {
                    Objects[x, z] = new WoodSpikeWall(tilebase + new Vec2i(x, z));

                }if(z==TileSize.z - 1)
                {
                    Objects[x, z] = new WoodSpikeWall(tilebase + new Vec2i(x, z));

                }
                Tiles[x, z] = Tile.DIRT.ID;
            }
        }

        EntityFaction banditFaction = new EntityFaction("Bandit_Camp");
        for(int x=0; x<Shell.Size.x; x++)
        {
            for (int z = 0; z < Shell.Size.z; z++)
            {
                //Entity e = new Bandit();
                //e.SetPosition(tilebase + new Vec2i(x * World.ChunkSize + 5, z * World.ChunkSize + z + 3));
                //Shell.AddEntity(x, z, e);
                //e.SetEntityFaction(banditFaction);
            }
        }
        Entity e = new Bandit();
        e.SetPosition(tilebase + new Vec2i(2 * World.ChunkSize + 5, 2 * World.ChunkSize + 2 + 3));
        Shell.AddEntity(0, 0, e);
        e.SetEntityFaction(banditFaction);



        return ToChunkData();

    }



    private List<ChunkData> ToChunkData()
    {
        List<ChunkData> data = new List<ChunkData>(Shell.Size.x * Shell.Size.z);
        for(int cx=0; cx<Shell.Size.x; cx++)
        {
            for (int cz = 0; cz < Shell.Size.z; cz++)
            {
                int[,] chunkTiles = new int[World.ChunkSize, World.ChunkSize];
                Dictionary<int, WorldObjectData> chunkObjs = new Dictionary<int, WorldObjectData>();

                for(int x=0; x<World.ChunkSize; x++)
                {
                    for (int z = 0; z < World.ChunkSize; z++)
                    {
                        int tx = cx * World.ChunkSize + x;
                        int tz = cz * World.ChunkSize + z;
                        chunkTiles[x, z] = Tiles[tx, tz];
                        if(Objects[tx,tz] != null)
                        {
                            chunkObjs.Add(WorldObject.ObjectPositionHash(x, z), Objects[tx, tz]);
                        }
                    }
                }
                data.Add(new ChunkData(Shell.Position.x + cx, Shell.Position.z + cz, chunkTiles, true, chunkObjs));
            }
        }
        return data;

    }

}