using UnityEngine;
using UnityEditor;

public class SteelGloves : HeavyArmor
{
    public override ItemID ID => ItemID.SteelGauntlets;

    public override string Name => "Steel Gloves";


    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.hands;

    public override int BaseProtection => 10;


    public override string SpriteTag => "test";

}