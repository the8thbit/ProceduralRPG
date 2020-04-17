using UnityEngine;
using UnityEditor;

public class SteelGloves : HeavyArmor
{
    public override int ID => STEEL_GLOVES;

    public override string Name => "Steel Gloves";

    public override bool IsEquiptable => true;

    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.hands;

    public override int BaseProtection => 10;


    public override string SpriteTag => "test";

}