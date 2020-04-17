using UnityEngine;
using UnityEditor;

public class NonAggresiveNPCCombatAI : EntityCombatAI
{
    public NonAggresiveNPCCombatAI()
    {
    }

    public override void WorldCombatEvent(WorldCombat wce)
    {
    }

    protected override void ChooseEquiptWeapon()
    {
    }

    protected override bool ShouldCombat(Entity entity)
    {
        return false;
    }

    protected override bool ShouldRun(Entity entity)
    {
        return false;
    }
}