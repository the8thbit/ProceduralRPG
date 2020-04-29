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

    public Vec2i Position { get; private set; }

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
        Position = (a.TilePos + b.TilePos) / 2;
    }

    public bool IsParticipant(Entity entity)
    {
        return Team1.Contains(entity) || Team2.Contains(entity);
    }

    /// <summary>
    /// Iterates all entities in team 1, and returns the one closest to
    /// 'source' entity
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Entity GetNearestTeam1Entity(Entity source)
    {
        Entity nearest = Team1[0];
        if (Team1.Count == 1)
            return nearest;
        int dist = Vec2i.QuickDistance(nearest.TilePos, source.TilePos);
        for(int i=1; i<Team1.Count; i++)
        {
            int nDist = Vec2i.QuickDistance(Team1[i].TilePos, source.TilePos);
            if(nDist < dist)
            {
                dist = nDist;
                nearest = Team1[i];
            }
        }
        return nearest;
    }
    /// <summary>
    /// Iterates all entities in team 2, and returns the one closest to
    /// 'source' entity
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Entity GetNearestTeam2Entity(Entity source)
    {
        Entity nearest = Team2[0];
        if (Team2.Count == 1)
            return nearest;
        int dist = Vec2i.QuickDistance(nearest.TilePos, source.TilePos);
        for (int i = 1; i < Team2.Count; i++)
        {
            int nDist = Vec2i.QuickDistance(Team2[i].TilePos, source.TilePos);
            if (nDist < dist)
            {
                dist = nDist;
                nearest = Team2[i];
            }
        }
        return nearest;
    }






}