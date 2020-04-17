using UnityEngine;
using UnityEditor;

public class QuestTask
{

    public bool IsCompleted { get; private set; }
    public bool IsCurrent { get; private set; }
    public string Description { get; private set; }
    public QuestTaskType TaskType { get; private set; }
    private object[] args;



    public QuestTask(string taskDescription, QuestTaskType type, object[] args)
    {
        IsCompleted = false;
        IsCurrent = false;

        Description = taskDescription;
        TaskType = type;
        this.args = args;
    }

    /// <summary>
    /// Should only be called if we have already checked the task type is 'TALK_TO_NPC'
    /// </summary>
    /// <returns></returns>
    public NPC GetNPC(int arg=0)
    {
        return args[arg] as NPC;
    }

    public Entity GetEntity(int arg = 0)
    {
        return args[arg] as Entity;
    }
    public Item GetItem(int arg = 0)
    {
        return args[arg] as Item;
    }

    public WorldMapLocation GetLocation(int arg = 0)
    {
        return args[arg] as WorldMapLocation;
    }

    public WorldMapLocation GetAssociatedLocation()
    {
        //Go to location tasks have arg[0] as location
        if (TaskType == QuestTaskType.GO_TO_LOCATION)
            return GetLocation();

        if (TaskType == QuestTaskType.TALK_TO_NPC)
            return GetNPC().NPCKingdomData.GetSettlement().WorldMapLocation;
        if (TaskType == QuestTaskType.PICK_UP_ITEM)
            return GetLocation(1);

        return null;

    }




    public override string ToString()
    {
        switch (TaskType)
        {
            case QuestTaskType.TALK_TO_NPC:
                return "Talk to " + GetNPC().Name;
            case QuestTaskType.PICK_UP_ITEM:
                return "Pick up " + GetItem().Name + " from " + GetLocation(1).ToString();
            case QuestTaskType.GO_TO_LOCATION:
                return "Go to " + GetLocation().ToString();
            case QuestTaskType.KILL_ENTITY:
                return "Kill " + GetEntity().Name;
        }
        return base.ToString();
    }


    public enum QuestTaskType
    {
        TALK_TO_NPC,
        PICK_UP_ITEM,
        GO_TO_LOCATION,
        KILL_ENTITY
    }
}