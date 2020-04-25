using UnityEngine;
using UnityEditor;
using System.Xml.Linq;
using System.Collections.Generic;
public class EntityGenerator
{
    public readonly World World;
    public readonly EntityManager EntityManager;
    public readonly GameGenerator GameGen;
    public EntityGenerator(GameGenerator gameGen, EntityManager entityManager)
    {
        GameGen = gameGen;
        World = gameGen.World;
        EntityManager = entityManager;
    }

    public void GenerateAllKingdomEntities()
    {
        foreach(KeyValuePair<int, Kingdom> kpv in World.WorldKingdoms)
        {
            KingdomNPCGenerator kingEntGen = new KingdomNPCGenerator(GameGen, kpv.Value, EntityManager);
            kingEntGen.GenerateKingdomNPC();

        }

       
    }

}