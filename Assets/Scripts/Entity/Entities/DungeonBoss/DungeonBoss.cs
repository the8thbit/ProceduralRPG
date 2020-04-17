using UnityEngine;
using UnityEditor;
[System.Serializable]
public abstract class DungeonBoss : HumanoidEntity
{
    public DungeonBoss(EntityCombatAI combatAI, EntityTaskAI taskAI, string name = "un-named_entity", bool isFixed = false) : base(combatAI, taskAI, name, isFixed)
    {
    }
}