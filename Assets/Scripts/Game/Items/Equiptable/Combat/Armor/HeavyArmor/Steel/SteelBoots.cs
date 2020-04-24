using UnityEngine;
using UnityEditor;

public class SteelBoots : HeavyArmor
{
    public override ItemID ID => ItemID.SteelBoots;

    public override string Name => "Steel Boots";


    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.feet;
    public override int BaseProtection => 10;

    public override string SpriteTag => "test";

}