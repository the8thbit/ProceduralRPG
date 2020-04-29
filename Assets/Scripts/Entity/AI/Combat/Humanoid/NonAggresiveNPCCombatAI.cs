using UnityEngine;
using UnityEditor;

/// <summary>
/// The standard Combat AI for the average NPC.
/// Will attack only when attacked, or if seeing a friend of theirs attacked.
/// </summary>
public class NonAggresiveNPCCombatAI : EntityCombatAI
{
    public NonAggresiveNPCCombatAI()
    {
    }
    
    public NPC NPC { get { return Entity as NPC; } }

    public override void OnDealDamage(Entity source)
    {

        
    }

    public override void WorldCombatEvent(WorldCombat wce)
    {
        //if we are already part of the combat event, we have no reaction.
        if (wce.IsParticipant(Entity))
            return;

        //if we are far, ignore
        if (wce.Position.QuickDistance(Entity.TilePos) > World.ChunkSize * World.ChunkSize * 9)
            return;

        if(CurrentCombatEvent == null || CurrentCombatEvent.IsComplete)
        {
            //If we are in the same faction as either of the sides, join accordingly
            if(Entity.EntityFaction != null && Entity.EntityFaction.Equals(wce.Faction1))
            {
                wce.Team1.Add(Entity);
                CurrentCombatEvent = wce;
                CurrentTarget = wce.GetNearestTeam2Entity(Entity);
                return;
            }else if(Entity.EntityFaction != null && Entity.EntityFaction.Equals(wce.Faction2))
            {
                wce.Team2.Add(Entity);
                CurrentCombatEvent = wce;
                CurrentTarget = wce.GetNearestTeam1Entity(Entity);
                return;
            }
            //if we are not in a faction/not related to their factions, we check friendships/family relations
            float team1RelVal = 0;
            bool team1High = false;

            float team2RelVal = 0;
            bool team2High = false;

            foreach (Entity e in wce.Team1)
            {
                float eRelVal = NPC.EntityRelationshipManager.GetEntityRelationship(e);
                //If we have a close friend/family member, take note
                if (eRelVal > 0.7f / NPC.EntityRelationshipManager.Personality.Loyalty)
                    team1High = true;
                team1RelVal += eRelVal;
            }
            
            foreach (Entity e in wce.Team2)
            {
                float eRelVal = NPC.EntityRelationshipManager.GetEntityRelationship(e);
                //If we have a close friend/family member, take note
                if (eRelVal > 0.7f/NPC.EntityRelationshipManager.Personality.Loyalty)
                    team2High = true;
                team2RelVal += eRelVal;
            }

            //If we have a close friend or family member, (Currently) do nothing.
            //TODO - add something here? Who knows?
            if (team1High && team2High)
            {
                return;
            }
            //If we have a family/friend on team 1, we join the combat with team 1.
            if (team1High)
            {
                //We join their team, then choose a target to fight
                wce.Team1.Add(Entity);
                CurrentCombatEvent = wce;
                CurrentTarget = wce.GetNearestTeam2Entity(Entity);
            }
            else if (team2High)
            {
                //We join their team, then choose a target to fight
                wce.Team2.Add(Entity);
                CurrentCombatEvent = wce;
                CurrentTarget = wce.GetNearestTeam1Entity(Entity);
            }
            else
            {
                //Check if aggression is high against RNG
                if (NPC.EntityRelationshipManager.Personality.Agression > GameManager.RNG.Random(0.6f, 0.8f))
                {
                    //Agression is between 0 and 1, we divide the team2 relationship value by this.
                    //And check against team1relval. This means higher agression requires less difference
                    //We also divide team2relval by the entities loyatly. This means that an entity with low loyalty requires a higher difference
                    if (team1RelVal > team2RelVal / (NPC.EntityRelationshipManager.Personality.Loyalty * NPC.EntityRelationshipManager.Personality.Agression))
                    {
                        wce.Team1.Add(Entity);
                        CurrentCombatEvent = wce;
                        CurrentTarget = wce.GetNearestTeam2Entity(Entity);
                        return;
                    }
                    else if (team2RelVal < team1RelVal / (NPC.EntityRelationshipManager.Personality.Loyalty * NPC.EntityRelationshipManager.Personality.Agression))
                    {
                        wce.Team2.Add(Entity);
                        CurrentCombatEvent = wce;
                        CurrentTarget = wce.GetNearestTeam1Entity(Entity);
                        return;
                    }


                }
                else
                {
                    Vec2i runPos = Entity.TilePos + GameManager.RNG.RandomVec2i(10, 20) * GameManager.RNG.RandomSign();
                    Entity.EntityAI?.TaskAI.SetTask(new EntityTaskGoto(Entity, runPos, priority: 10, running: true));
                }//if our agression is low, we check for 
                //If neither team has a friend/family, we 

            }



        }       

    }

    protected override void ChooseEquiptWeapon()
    {
    }

    protected override bool ShouldCombat(Entity entity)
    {
        return false;
    }

    protected override bool ShouldRun(Entity entity)
    {
        return false;
    }
}