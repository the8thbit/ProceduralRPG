using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
[System.Serializable]
public class EntityAI
{
    private Entity Entity;
    public EntityCombatAI CombatAI;
    public EntityTaskAI TaskAI;



    private Vec2i EntityPathTarget;
    public EntityPath EntityPath; //TODO - change back to private

    public EntityAI(Entity entity, EntityCombatAI combatAI, EntityTaskAI taskAI)
    {
        Entity = entity;
        CombatAI = combatAI;
        TaskAI = taskAI;
    }

    /// <summary>
    /// Update is called every frame
    /// </summary>
    public void Update()
    {
        if (CombatAI.InCombat)
            CombatAI.Update();
        else
            TaskAI.Update();

    }
    //Tick is called not every frame
    public void Tick()
    {
        CombatAI.Tick();
        if (!CombatAI.InCombat)
            TaskAI.Tick();
    }


    public bool GeneratePath(Vec2i target)
    {
        //if no target, null previous paths and return.
        if (target == null)
        {
            EntityPathTarget = null;
            EntityPath = null;
            return false;
        }
        Vec2i tilePos = Vec2i.FromVector3(Entity.Position);
        if (EntityPath == null)
        {
            //If there is no path, Attempt to generate it
            List<Vec2i> path = GameManager.PathFinder.GeneratePath(tilePos, target);
            if (path.Count == 0)
            {

                return false;
            }
            EntityPathTarget = target;
            EntityPath = new EntityPath(path);
            return true;
        }
        else if(EntityPath.Target == target)
        {//If the current path has the same target, return true
            return true;
        }
        else
        {
            if(Vec2i.QuickDistance(EntityPath.Target, target) < 5)
            {
                //If the path exists but doesn't have the same target, but targets are close, we attempt to re-calculate the path
                return EntityPath.UpdateTarget(target);
            }else
            {
                EntityPath = null;
                List<Vec2i> newPath = GameManager.PathFinder.GeneratePath(tilePos, target);
                if(newPath.Count == 0)
                {
                    Debug.Log("No path found from " + tilePos + " to " + target + " for entity " + Entity);
                    return false;
                }
                EntityPath = new EntityPath(newPath);
                return true;
            }
            
        }        
    }

    
    public void FollowPath()
    {
        if (EntityPath == null)
            return;
        Vector2 next = EntityPath.CurrentIndex().AsVector2();
        if (PositionWithinDistance(Entity.GetLoadedEntity().transform.position, next, 0.1f))
        {
            Vec2i nexti = EntityPath.NextIndex();
            if (nexti == null)
                return;
            next = nexti.AsVector2();
        }
        
        if(next != null && Entity.GetLoadedEntity()!=null) {
            
            Entity.GetLoadedEntity().MoveTowards(next);
        }
    }

    /// <summary>
    /// Called when entity is loaded into game. Adds event listeners
    /// </summary>
    public void OnEntityLoad()
    {
        GameManager.EventManager.AddListener(CombatAI);
        CombatAI.CheckEquiptment();
        CombatAI.SetEntity(Entity);
        TaskAI.SetEntity(Entity);
        //GameManager.EventManager.AddListener(TaskAI);
    }
    /// <summary>
    /// Called when an entity is unloaded or killed, removes listeners
    /// </summary>
    public void OnEntityUnload()
    {
        GameManager.EventManager.RemoveListener(CombatAI);

    }


    private static bool PositionWithinDistance(Vector3 a, Vector2 b, float dist)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.z - b.y) * (a.z - b.y) < dist * dist;
    }
    private static bool Vector2WithinDistance(Vector2 a, Vector2 b, float dist)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) < dist * dist;
    }
}