using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
public interface IWorldCombatEvent : IEventListener
{

    void WorldCombatEvent(WorldCombat wce);

}
/// <summary>
/// Called when entities enter combat.
/// </summary>
[System.Serializable]
public class WorldCombat : IEvent
{
    public List<Entity> Team1 { get; private set; }
    public List<Entity> Team2 { get; private set; }
    public EntityFaction Faction1 { get; private set; }
    public EntityFaction Faction2 { get; private set; }

    public bool IsComplete { get; protected set; }


    public WorldCombat(Entity a, Entity b)
    {  

        Team1 = new List<Entity>();
        Team2 = new List<Entity>();
        Team1.Add(a);
        Team2.Add(b);
        Faction1 = a.EntityFaction;
        Faction2 = b.EntityFaction;
        IsComplete = false;
    }




   


}