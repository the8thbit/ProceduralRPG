using UnityEngine;
using UnityEditor;

public class SteelBoots : HeavyArmor
{
    public override int ID => STEEL_BOOTS;

    public override string Name => "Steel Boots";

    public override bool IsEquiptable => true;

    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.feet;
    public override int BaseProtection => 10;

    public override string SpriteTag => "test";
}