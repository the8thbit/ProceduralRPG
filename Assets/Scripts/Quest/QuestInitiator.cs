using UnityEngine;
using UnityEditor;

/// <summary>
/// The initial event needed to start a quest
/// </summary>
public class QuestInitiator
{

    public InitiationType InitType { get; private set; }
    private object[] args;
    public QuestInitiator(InitiationType init, object[] args)
    {
        InitType = init;
        this.args = args;
    }

    /// <summary>
    /// Only to be called if we have alreadyed checked the initiation type to be "TALK_TO_NPC"
    /// </summary>
    /// <returns></returns>
    public NPC GetNPC(int arg=0)
    {
        return args[arg] as NPC;
    }
    /// <summary>
    /// Only to be called if we have alreadyed checked the initiation type to be "PICK_UP_ITEM"
    /// </summary>
    /// <returns></returns>
    public Item GetItem(int arg = 0)
    {
        return args[arg] as Item;
    }

    public Vec2i GetLocation(int arg = 0)
    {
        return args[arg] as Vec2i;
    }


    public T GetArg<T>(int id)
    {
        return (T)args[id];
    }

    public override string ToString()
    {
        string total = "";

        switch (InitType)
        {
            case InitiationType.TALK_TO_NPC:
                total = "Talk to " + GetNPC().Name + " at " + Vec2i.FromVector3(GetNPC().Position);
                break;
            case InitiationType.PICK_UP_ITEM:
                total = "Pick up item " + GetItem().Name + " at " + GetLocation(1);
                break;

        }

        return total;


    }

    public enum InitiationType
    {
        TALK_TO_NPC,
        PICK_UP_ITEM,
        NPC_DIALOG_OPTION,
        EXTERNAL_SET
    }
}