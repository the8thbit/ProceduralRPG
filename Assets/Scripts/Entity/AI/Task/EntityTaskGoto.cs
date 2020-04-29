using UnityEngine;
using UnityEditor;

/// <summary>
/// This task attempts to get the entity to travel from a start location to an end location
/// </summary>
[System.Serializable]
public class EntityTaskGoto : EntityTask
{

    private Vec2i TargetTile;
    private bool Running;
    public EntityTaskGoto(Entity entity, Building building, bool running=false) : base(entity, 5, -1)
    {
        TargetTile = GameManager.RNG.RandomFromList(building.GetSpawnableTiles());
        Running = running;
    }
    public EntityTaskGoto(Entity entity, Vec2i targetTile, float priority=5, bool running=false) : base (entity, priority, -1)
    {
        TargetTile = targetTile;
        Running = running;
    }

    public override void Update()
    {
       
        if (Entity.EntityAI.EntityPath == null)
            return;
        if (Running)
            Entity.GetLoadedEntity().SetRunning(true);
        Entity.EntityAI.FollowPath();
        
    }

    public override void OnTaskEnd()
    {
        if (Running)
            Entity.GetLoadedEntity().SetRunning(false);
    }

    protected override void InternalTick()
    {
        if (Entity.TilePos == TargetTile)
        {
            IsComplete = true;
            return;
        }
        
        int quickDist = QuickDistanceToPlayer(Entity);
        //If we are currently far from the player
        if(quickDist > (World.ChunkSize * 3) * (World.ChunkSize * 3))
        {
            //And the target position is far from the player
            if(Vec2i.QuickDistance(TargetTile, GameManager.PlayerManager.Player.TilePos) > (World.ChunkSize * 3) * (World.ChunkSize * 3))
            {
                //Then we teleport to our target position.
                Entity.SetPosition(TargetTile);
                IsComplete = true;
                return;
            }
        }

        Entity.EntityAI.GeneratePath(TargetTile);
        
    }
}