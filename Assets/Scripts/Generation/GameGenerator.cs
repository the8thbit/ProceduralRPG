using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGenerator
{
    public static Texture2D MAP;
    public int Seed { get; private set; }

    public World World { get; private set; }

    public GenerationRandom MainGenerator;
    public TerrainGenerator TerrainGenerator;
    public RiverGenerator RiverGenerator;
    public LakeGenerator LakeGenerator;
    public ChunkGenerator ChunkGenerator;
    public ChunkStructureGenerator StructureGenerator;
    private Dictionary<Vec2i, ChunkData> PreGeneratedChunks;

    public GameGenerator(int seed)
    {
        Seed = seed;
    }

    /// <summary>
    /// Generates the world.
    /// Initiates a <see cref="TerrainGenerator"/> & generates the base Terrain <see cref="TerrainGenerator.GenerateBaseTerrain"/>
    /// Then Initiates a <see cref="KingdomsGenerator"/> that generates all the kingdoms, as well as claiming all their territory
    /// Then generates terrain details using <see cref="TerrainGenerator.GenerateWorldDetailTerrain"/>
    /// Finally, generates all settlements using <see cref="KingdomsGenerator.GenerateAllKingdomSettlements"/>
    /// 
    /// TODO - Add dungeon generation, add settlement details generation (roads + paths)
    /// </summary>
    /// <returns></returns>
    public void GenerateWorld(WorldManager wm)
    {
        //Set Random init state based on seed
        Random.InitState(Seed);
        MiscMaths.SetRandomSeed(Seed);
        //Initiate empty world and generate base terrain
        World = new World();
        wm.SetWorld(World);


        MainGenerator = new GenerationRandom(Seed);
        TerrainGenerator = new TerrainGenerator(this, World);
        RiverGenerator = new RiverGenerator(this);
        LakeGenerator = new LakeGenerator(this);


        Debug.BeginDeepProfile("chunk_base_gen");

        TerrainGenerator.GenerateChunkBases();//Generate the chunk bases, these only define if they are land or sea.
        TerrainGenerator.GenerateTerrainDetails(); //generate rivers
        
        Debug.EndDeepProfile("chunk_base_gen");



        ChunkGenerator = new ChunkGenerator(this);



        Debug.BeginDeepProfile("kingdom_set_gen");
        //We then generate empty kingdoms based on these empty chunks
        KingdomsGenerator kingGen = new KingdomsGenerator(World, this);
        //We also generate the all empty settlements for the world.
        kingGen.GenerateEmptyKingdomsFromBaseChunks(4);
        //Generates all settlements
        PreGeneratedChunks = kingGen.GenerateSettlements();

        Debug.EndDeepProfile("kingdom_set_gen");


        Debug.BeginDeepProfile("chunk_struct_gen");

        StructureGenerator = new ChunkStructureGenerator(this);
        StructureGenerator.GenerateStructureShells();
       foreach(KeyValuePair<Vec2i, ChunkData> kvp in StructureGenerator.GenerateAllStructures())
        {
            PreGeneratedChunks.Add(kvp.Key, kvp.Value);
        }
        Debug.EndDeepProfile("chunk_struct_gen");


        Debug.BeginDeepProfile("terrain_map_gen");
        GenerateTerrainMap(TerrainGenerator.ChunkBases);
        Debug.EndDeepProfile("terrain_map_gen");

    }

    public ChunkRegionGenerator GenerateChunks(Vec2i midpoint)
    {
        Debug.BeginDeepProfile("start_region_gen");
        ChunkRegionGenerator crg = new ChunkRegionGenerator(this, PreGeneratedChunks);
        crg.GenStartRegion(midpoint);

        

        Debug.EndDeepProfile("start_region_gen");

        return crg;
    }

    private void GenerateTerrainMap(ChunkBase[,] cbs)
    {
        Texture2D terMap = new Texture2D(World.WorldSize, World.WorldSize);
        for (int x = 0; x < World.WorldSize; x++)
        {
            for (int z = 0; z < World.WorldSize; z++)
            {
                ChunkBase cb = cbs[x, z];
                Color c = Color.black;
                switch (cb.Biome)
                {
                    case ChunkBiome.ocean:
                        c = new Color(0, 0, 1);
                        break;
                    case ChunkBiome.grassland:
                        c = new Color(0, 1, 0);
                        break;
                    case ChunkBiome.dessert:
                        c = new Color(1, 1, 0);
                        break;
                    case ChunkBiome.forrest:
                        c = new Color(34f / 255f, 139f / 255f, 34f / 255f);
                        break;
                }

                if (cb.RiverNode != null)
                    c = new Color(0, 191f / 255f, 1f);
                if(cb.Lake != null)
                {
                    c = new Color(0, 0.2f, 1f);
                }
                if (cb.ChunkStructure != null)
                {
                    c = Color.magenta;
                    //Debug.Log("chunk struct at " + x + "_" + z);
                }
                    
                terMap.SetPixel(x, z, c);
            }
        }
        terMap.Apply();
        GameManager.Game.toDrawTexts[3] = terMap;
    }

    public void GenerateDungeons()
    {
        Debug.BeginDeepProfile("dungeon_gen");


        List<DungeonEntrance> dungeonEntrances = new List<DungeonEntrance>();
        //Iterate all chunk structures, if they have a dungeon entrance add it to the list

        foreach(KeyValuePair<Vec2i, ChunkStructure> kvp in StructureGenerator.ChunkStructureShells)
        {
            if (kvp.Value.HasDungeonEntrance)
            {
                dungeonEntrances.Add(kvp.Value.DungeonEntrance);
            }
        }

        DungeonGenerator dungGen = new DungeonGenerator(this);
        List<Dungeon> genDun = dungGen.GenerateAllDungeons(dungeonEntrances);
        foreach(Dungeon d in genDun)
        {
            World.AddSubworld(d);
        }

        Debug.EndDeepProfile("dungeon_gen");


    }

    public void GenerateWorldMap()
    {
        World.SetChunkBases(TerrainGenerator.ChunkBases);
        World.CreateWorldMap();
    }

    private Dictionary<Kingdom, Color> KingdomColors;

    
    private Color GetKingdomColor(Kingdom k)
    {
        if (!KingdomColors.ContainsKey(k))
        {
            KingdomColors.Add(k, new Color(Random.value, Random.value, Random.value));
        }
        return KingdomColors[k];
    }
    private Color GetSettlementColor(SettlementBase set)
    {
        switch (set.SettlementType)
        {
            case SettlementType.CAPITAL:
                return Color.red;
            case SettlementType.CITY:
                return Color.yellow;
            case SettlementType.TOWN:
                return Color.magenta;
            case SettlementType.VILLAGE:
                return Color.white;
        }
        return Color.black;
    }


    /// <summary>
    /// Generates all entities using <see cref="EntityGenerator.GenerateAllKingdomEntities"/>
    /// </summary>
    /// <param name="world"></param>
    public void GenerateEntities(World world)
    {
        Debug.BeginDeepProfile("entity_gen");

        //initiate a new instance of EntityGenerator, and then use this to general all entities that belong to a kingdom (most NPCs)
        EntityGenerator entityGen = new EntityGenerator(world, GameManager.EntityManager);
        entityGen.GenerateAllKingdomEntities();


        Debug.EndDeepProfile("entity_gen");

    }


    /// <summary>
    /// Generates all quests for the game using <see cref="QuestGenerator.GenerateAllQuests"/>
    /// </summary>
    /// <param name="world"></param>
    /// <returns></returns>
    public List<Quest> GenerateQuests(World world)
    {
        //Creates new instance of quest generator and use it to generate all the quests
        QuestGenerator questGen = new QuestGenerator(world);
        return questGen.GenerateAllQuests();
    }

}
