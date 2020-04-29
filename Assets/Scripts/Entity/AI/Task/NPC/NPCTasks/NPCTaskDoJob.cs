using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
[System.Serializable]
public class NPCTaskDoJob : EntityTask
{
    private Building WorkBuilding;
    private WorkEquiptmentData WorkEquiptment;
    bool CanWork;
    public NPCTaskDoJob(Entity entity, IWorkBuilding building,  float priority, float taskTime = -1) : base(entity, priority, taskTime)
    {
        WorkBuilding = building.WorkBuilding;
        List<WorkEquiptmentData> wed = new List<WorkEquiptmentData>(10);
        foreach(WorldObjectData wed_ in WorkBuilding.GetBuildingObjects())
        {
            if (wed_ is WorkEquiptmentData && (wed_ as WorkEquiptmentData).CurrentUser==null)
                wed.Add(wed_ as WorkEquiptmentData);
        }
        WorkEquiptment = GameManager.RNG.RandomFromList(wed);
        

        if(WorkEquiptment != null)
        {
            WorkEquiptment.CurrentUser = Entity as NPC;
            CanWork = QuickDistance(Entity, WorkEquiptment.WorldPosition) <= 1;

        }
        else
        {
            CanWork = true;
        }

    }

    public override void Update()
    {
        if (CanWork)
        {
            Debug.Log("working");
            Entity.GetLoadedEntity().Jump();
        }
    }

    protected override void InternalTick()
    {
        if(WorkEquiptment != null)
        {
            if (!CanWork)
            {

                Debug.Log("Work equiptment at " + WorkEquiptment.WorldPosition);

                if (Entity.EntityAI.GeneratePath(WorkEquiptment.WorldPosition))
                {
                    if (Entity.EntityAI.FollowPath())
                        CanWork = true;
                }
            }
        }
 
    }
}