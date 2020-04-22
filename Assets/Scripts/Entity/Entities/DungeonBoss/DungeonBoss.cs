using UnityEngine;
using UnityEditor;
[System.Serializable]
public abstract class DungeonBoss : HumanoidEntity
{
    public DungeonBoss(EntityCombatAI combatAI, EntityTaskAI taskAI, EntityMovementData movementData = default(EntityMovementData), 
        string name = "un-named_entity", bool isFixed = false) 
        : base(combatAI, taskAI, movementData, name, isFixed)
    {
    }
}