using UnityEngine;
using UnityEditor;
[System.Serializable]
public class DungeonBossTest : DungeonBoss
{
    public DungeonBossTest(EntityCombatAI combatAI, EntityTaskAI taskAI, string name = "un-named_entity", bool isFixed = false) : base(combatAI, taskAI, name, isFixed)
    {
    }

    public override string EntityGameObjectSource => "human";

    protected override void KillInternal()
    {
        
    }
}
