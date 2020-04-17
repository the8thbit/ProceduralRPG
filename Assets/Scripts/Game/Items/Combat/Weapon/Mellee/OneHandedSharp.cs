using UnityEngine;
using UnityEditor;
[System.Serializable]
public abstract class OneHandedSharp : Weapon
{
    public OneHandedSharp(ItemMetaData meta = null) : base(meta)
    {
    }


    public override DamageType DamageType => DamageType.SHARP;


    public override bool IsTwoHanded => false;



    public override bool IsEquiptable => true;

    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.weapon;
}