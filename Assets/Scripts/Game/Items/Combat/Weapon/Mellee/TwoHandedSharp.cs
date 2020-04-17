using UnityEngine;
using UnityEditor;

public abstract class TwoHandedSharp : Weapon
{
    public TwoHandedSharp(ItemMetaData meta = null) : base(meta) { }
    public override float Damage => 10;

    public override DamageType DamageType => DamageType.SHARP;

    public override bool IsTwoHanded => true;



    public override bool IsEquiptable => true;
    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.weapon;
}