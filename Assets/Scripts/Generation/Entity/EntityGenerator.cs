using UnityEngine;
using UnityEditor;
using System.Xml.Linq;
using System.Collections.Generic;
public class EntityGenerator
{
    public readonly World World;
    public readonly EntityManager EntityManager;

    public EntityGenerator(World world, EntityManager entityManager)
    {
        World = world;
        EntityManager = entityManager;
    }

    public void GenerateAllKingdomEntities()
    {
        foreach(KeyValuePair<int, Kingdom> kpv in World.WorldKingdoms)
        {
            KingdomNPCGenerator kingEntGen = new KingdomNPCGenerator(kpv.Value, EntityManager);
            kingEntGen.GenerateKingdomNPC();

        }

       
    }

}