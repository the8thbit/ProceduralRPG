using UnityEngine;
using UnityEditor;

public class SteelLongSword : OneHandedSharp
{
    public SteelLongSword(ItemMetaData meta = null) : base(meta) { }
    public override float Damage => 50;

    public override int ID => STEEL_LONG_SWORD;

    public override string Name => "Steel Long Sword";

    public override string SpriteTag => "SteelLongSword";

    public override float AttackStaminaUse => 20;

    public override float WeaponRange => 1.5f;

    public override float WeaponCooldown => 2;

    public override float WeaponAttackTime => 1;
}