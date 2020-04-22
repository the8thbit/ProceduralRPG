﻿using UnityEngine;
using UnityEditor;
[System.Serializable]
public class DungeonBossTest : DungeonBoss
{
    public DungeonBossTest(EntityCombatAI combatAI, EntityTaskAI taskAI, EntityMovementData movementData = default(EntityMovementData), 
        string name = "un-named_entity", bool isFixed = false) 
        : base(combatAI, taskAI,movementData, name, isFixed)
    {
    }

    public override string EntityGameObjectSource => "human";

    protected override void KillInternal()
    {
        
    }
}
