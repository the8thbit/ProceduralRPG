using UnityEngine;
using UnityEditor;

public class SteelLegs : HeavyArmor
{
    public override ItemID ID => ItemID.SteelLegs;

    public override string Name => "Steel Legs";


    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.legs;
    public override int BaseProtection => 10;
    public override string SpriteTag => "SteelLegs";

}