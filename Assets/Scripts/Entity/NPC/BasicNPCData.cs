using UnityEngine;
using UnityEditor;
/// <summary>
/// Holds onto various basic data for an NPC
/// </summary>
/// 
[System.Serializable]
public class BasicNPCData
{

    public NPCJob NPCJob { get; private set; }
    public bool HasJob { get { return NPCJob != null; } }
    public void SetJob(NPCJob job)
    {
        NPCJob = job;
    }

    public Building House { get; private set; }
    public bool HasHouse { get { return House != null; } }
    public void SetHome(House house)
    {
        House = house;
    }

    //Debug, TODO add check for if npc should go to work.
    public bool ShouldGoToWork { get { return NPCJob != null; } }


    public override string ToString()
    {

        return base.ToString();
    }
}

