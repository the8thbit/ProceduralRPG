using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
public class KingdomNPCGenerator
{
    public readonly Kingdom Kingdom;
    public readonly EntityManager EntityManager;
    public KingdomNPCGenerator(Kingdom kingdom, EntityManager entityManager)
    {
        Kingdom = kingdom;
        EntityManager = entityManager;
    }


    public void GenerateKingdomNPC()
    {
        foreach(int i in Kingdom.SettlementIDs)
        {

            GenerateSettlementNPC(GameManager.WorldManager.World.GetSettlement(i));
        }

    }

    /// <summary>
    /// Generates all NPCs in a settlement
    /// First, generates each empty NPC <see cref="GenerateNPCShells(Settlement)"/>
    /// Next, generates the NPCKingdomData <see cref="GenerateSettlementNPCKingdomData(List{NPC}, Settlement)"/>
    /// Then sets the entities job
    /// </summary>
    /// <param name="set"></param>
    public void GenerateSettlementNPC(Settlement set)
    {
        
        List<NPC> npcs = GenerateNPCShells(set);
        GenerateSettlementNPCKingdomData(npcs, set);
        GenerateNPCJobs(npcs, set);
        GenerateNPCDialoges(npcs);
        return;
    }
    /// <summary>
    /// Generates all empty NPCs, defining only their name, and thier homes
    /// </summary>
    /// <param name="set"></param>
    private List<NPC> GenerateNPCShells(Settlement set)
    {
        if (set.Buildings == null)
            return null;
        List<NPC> settlementNPCs = new List<NPC>(250);
        //iterate all building, and check if this is a house
        foreach(Building b in set.Buildings)
        {
            if(b is House)
            {
                //Define the house and get the possible tiles we can spawn the this houses' NPCs on
                House h = b as House;
                List<Vec2i> spawnableTiles = h.GetSpawnableTiles();
                
                for(int i=0; i<h.capacity; i++)
                {
                    //Attempt to find a valid spawn point
                    Vec2i entitySpawn = MiscUtils.RandomFromArray(spawnableTiles, Random.value);
                    if(entitySpawn == null)
                    {
                        Debug.Log("No valid spawn point found for house " + h.ToString(), Debug.ENTITY_GENERATION);
                        continue;
                    }
                    //Create the empty NPC and add it to the settlement
                    NPC npc = new NPC(isFixed: true);
                    npc.SetPosition(entitySpawn);
                    npc.NPCData.SetHome(h);
                    EntityManager.AddFixedEntity(npc);
                    npc.SetName("NPC " + npc.ID);
                    set.AddNPC(npc);
                    settlementNPCs.Add(npc);
                }
            }
        }
        return settlementNPCs;
    }
    /// <summary>
    /// Generates the kingdom data for each NPC -
    /// Tells the NPC which settlement and kingdom it belongs too, as well as its position in the settlement
    /// </summary>
    /// <param name="npcs"></param>
    /// <param name="settlement"></param>
    private void GenerateSettlementNPCKingdomData(List<NPC> npcs, Settlement settlement)
    {    
        foreach(NPC npc in npcs)
        {
            KingdomHierarchy rank = KingdomHierarchy.Peasant;
            House h = npc.NPCData.House as House;
            if(h is Castle) //Check if the house is a Castle
            {
                //If this is a capital, then this NPC is a monarch
                if (settlement.SettlementType == SettlementType.CAPITAL)
                {
                    rank = KingdomHierarchy.Monarch;
                }
                else
                {//If not, they're a noble
                    rank = KingdomHierarchy.Noble;
                }
                //Either way, they'll be a leader of this settlement
                settlement.AddLeader(npc);
            }
            else if(h is Hold)
            {
                rank = KingdomHierarchy.LordLady;
                settlement.AddLeader(npc);
            }
            else if(h is VillageHall)
            {
                rank = KingdomHierarchy.Mayor;
                settlement.AddLeader(npc);
            }
            else
            {
                if(settlement.SettlementType == SettlementType.CAPITAL)
                {
                    rank = KingdomHierarchy.Citizen; //All NPCs in capital are citizens
                }
                else
                {
                    //Citizen or peasant based on house value
                    float cutoff = 150; //The cutoff value. If house is worth less than this, peasant
                    if(h.Value < cutoff)
                    {
                        rank = KingdomHierarchy.Peasant;
                    }
                    else
                    {
                        rank = KingdomHierarchy.Citizen;
                    }
                }
            }
            //Create and appl.
            NPCKingdomData kData = new NPCKingdomData(rank, settlement.KingdomID, settlement.SettlementID);
            npc.SetKingdomData(kData);
        }
    }
    
    private void GenerateNPCJobs(List<NPC> npcs, Settlement set)
    {
        List<NPCJob> workJobs = new List<NPCJob>();
        Dictionary<KingdomHierarchy, List<NPCJob>> rankedJobs = new Dictionary<KingdomHierarchy, List<NPCJob>>();
        //Iterate all buildings and select work buildings
        foreach (Building b in set.Buildings)
        {
            if (b is WorkBuilding)
            {
                //Get the job for each building
                List<NPCJob> js = (b as WorkBuilding).GetJobs();
                foreach(NPCJob j in js)
                {
                    if (!rankedJobs.ContainsKey(j.RequiredRank))
                        rankedJobs.Add(j.RequiredRank, new List<NPCJob>());
                    rankedJobs[j.RequiredRank].Add(j);
                }
      
            }
        }
        Debug.Log("Settlement " + set.ToString() + " has " + workJobs.Count + " jobs", Debug.ENTITY_GENERATION);


        foreach (NPC npc in npcs)
        {
            KingdomHierarchy rank = npc.NPCKingdomData.Rank;
            if (!rankedJobs.ContainsKey(rank))
                continue;
            if (rankedJobs[rank].Count == 0)
                continue;

            NPCJob job = rankedJobs[rank][0];
            rankedJobs[rank].Remove(job);
            npc.NPCData.SetJob(job);
        }
    }

    /// <summary>
    /// Generates a dialoge tree for all NPCs
    /// </summary>
    /// <param name="npcs"></param>
    private void GenerateNPCDialoges(List<NPC> npcs)
    {
        //NPCDialog dialog = new NPCDialog()

        foreach(NPC npc in npcs)
        {

        }
    }
}