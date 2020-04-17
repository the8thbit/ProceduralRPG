using UnityEngine;
using UnityEditor;

public class NPCRelationship
{
    public int TargetNPC { get; private set; }
    public int RelationshipValue { get; private set; }
    public NPCRelationship(int targetNPC, int value)
    {
        TargetNPC = targetNPC;
        RelationshipValue = value;
    }

    public void SetRelationshipValue(int v)
    {
        RelationshipValue = v;
    }
    public void ChangeRelationshipValue(int delta)
    {
        RelationshipValue += delta;
        if (RelationshipValue < 0)
            RelationshipValue = 0;
        else if (RelationshipValue > 100)
            RelationshipValue = 100;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (!(obj is NPCRelationship))
            return false;
        return (obj as NPCRelationship).TargetNPC == TargetNPC;
    }
}