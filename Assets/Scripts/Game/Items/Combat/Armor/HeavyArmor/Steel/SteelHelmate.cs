using UnityEngine;
using UnityEditor;

public class SteelHelmate : HeavyArmor
{
    public override int ID => STEEL_HELM;

    public override string Name => "Steel Helmate";

    public override bool IsEquiptable => true;

    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.head;
    public override int BaseProtection => 10;
    public override string SpriteTag => "test";

}