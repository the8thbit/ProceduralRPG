using UnityEngine;
using UnityEditor;

[System.Serializable]
public class SpellFireball : SingleSpell
{
    public override float ManaCost => 20;

    public override string Description => "A simple fire ball spell";


    public override float CoolDown => 3;

    public override float XPGain => 5;

    public override SpellCombatType SpellCombatType => SpellCombatType.OFFENSIVE;

    public override void CastSpell(SpellCastData data)
    {
        Vector2 look = new Vector2(Mathf.Cos(data.Source.LookAngle * Mathf.Deg2Rad), -Mathf.Sin(data.Source.LookAngle * Mathf.Deg2Rad));
        GameManager.WorldManager.AddNewProjectile(data.Source.Position + Vector3.up * 0.8f, look, new FireBall(), data.Source);
    }
}