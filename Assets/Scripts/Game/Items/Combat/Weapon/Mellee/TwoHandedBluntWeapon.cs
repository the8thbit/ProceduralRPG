using UnityEngine;
using UnityEditor;

public abstract class TwoHandedBluntWeapon : Weapon
{
    public TwoHandedBluntWeapon(ItemMetaData meta = null) : base(meta) { }

    public override DamageType DamageType => DamageType.BLUNT;

    public override bool IsTwoHanded => true;


    public override bool IsEquiptable => true;

    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.weapon;
}