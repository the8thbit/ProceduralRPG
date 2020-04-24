using UnityEngine;
using UnityEditor;

public abstract class OneHandedBluntWeapon : Weapon
{
    public OneHandedBluntWeapon(ItemMetaData meta=null) : base(meta) { }
    public override DamageType DamageType => DamageType.BLUNT;

    public override bool IsTwoHanded => false;


    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.weapon;
}