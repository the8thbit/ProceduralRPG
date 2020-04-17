using UnityEngine;
using UnityEditor;

public abstract class RangeWeapon : Weapon
{
    public RangeWeapon(ItemMetaData meta=null) : base(meta)    { }


    public override DamageType DamageType => DamageType.SHARP;

    public abstract Projectile GenerateProjectile();


    public override bool IsEquiptable => true;

    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.weapon;
}