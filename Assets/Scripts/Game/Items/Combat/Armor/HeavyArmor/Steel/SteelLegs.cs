using UnityEngine;
using UnityEditor;

public class SteelLegs : HeavyArmor
{
    public override int ID => STEEL_LEGS;

    public override string Name => "Steel Legs";

    public override bool IsEquiptable => true;

    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.legs;
    public override int BaseProtection => 10;
    public override string SpriteTag => "test";

}