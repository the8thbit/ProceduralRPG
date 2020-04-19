using UnityEngine;
using UnityEditor;
[System.Serializable]
public class SpellFireBreath : HoldSpell
{
    public override float ManaCost => 5f;

    public override string Description => "Breath Fire";

    public override float XPGain => 1;

    public override SpellCombatType SpellCombatType => SpellCombatType.OFFENSIVE;

    private LoadedBeam LoadedSpell;

    public override void SpellEnd(SpellCastData data)
    {
        GameManager.WorldManager.DestroyBeam(LoadedSpell);
        IsCast = false;
    }

    public override void SpellStart(SpellCastData data)
    {
        Beam beam = new BeamFireBreath(this);
        LoadedSpell = GameManager.WorldManager.CreateNewBeam(data.Source, beam, data.Target);
        IsCast = true;
    }

    public override void SpellUpdate(SpellCastData data)
    {
        if(LoadedSpell != null && IsCast)
            LoadedSpell.UpdateTarget(data.Target);
    }
}