using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
public class BanditCaveDungeonBuilder : DungeonBuilder
{
    public BanditCaveDungeonBuilder(Vec2i chunkPos, Vec2i size) : base(chunkPos, size, DungeonType.CAVE)
    {
    }

    public override Dungeon Generate(DungeonEntrance entr, GenerationRandom ran = null)
    {
        if (ran == null)
        {
            ran = new GenerationRandom(ChunkPosition.x << 16 + ChunkPosition.z);
        }


        ChunkData[,] chunks = CreateChunks();
        Vec2i dunEntr = ChunkPosition * World.ChunkSize + new Vec2i(World.ChunkSize / 2, World.ChunkSize / 2);
        List<Entity> dunEnt = GenerateEntities();
        DungeonBoss dunBoss = GenerateDungeonBoss();


        return new Dungeon(chunks, new Vec2i(5, 5), dunEntr, dunEnt, dunBoss);
    }



    protected override DungeonBoss GenerateDungeonBoss()
    {
        return new DungeonBossTest(new BasicHumanoidCombatAI(), new CreatureTaskAI());
    }

    protected override List<Entity> GenerateEntities()
    {
        List<Entity> entities = new List<Entity>();
        return entities;
    }
}