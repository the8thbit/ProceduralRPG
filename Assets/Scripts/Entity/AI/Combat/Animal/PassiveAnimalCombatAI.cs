using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PassiveAnimalCombatAI : EntityCombatAI
{


    public override void Update()
    {
    }

    public override void WorldCombatEvent(WorldCombat wce)
    {
        
    }

    /// <summary>
    /// No weapon for a passive animal sadly.
    /// </summary>
    protected override void ChooseEquiptWeapon()
    {

    }

    protected override bool ShouldCombat(Entity entity)
    {
        return false;
    }

    protected override bool ShouldRun(Entity entity)
    {
        return true;
    }
}