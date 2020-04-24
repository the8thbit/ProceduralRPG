using UnityEngine;
using UnityEditor;

public class SteelHelmate : HeavyArmor
{
    public override ItemID ID => ItemID.SteelHelm;

    public override string Name => "Steel Helmate";


    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.head;
    public override int BaseProtection => 10;
    public override string SpriteTag => "test";

}