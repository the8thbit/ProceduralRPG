using UnityEngine;
using UnityEditor;

public class ShortBow : RangeWeapon
{
    public override float Damage => 10;

    public override float WeaponCooldown => 1;

    public override float AttackStaminaUse =>5;

    public override float WeaponRange => 40;

    public override bool IsTwoHanded => true;

    public override int ID => SHORT_BOW;

    public override string Name => "Short Bow";

    public override string SpriteTag => "ShortBow";

    public override float WeaponAttackTime => 1;

    public override Projectile GenerateProjectile()
    {
        return new Arrow();
    }
}