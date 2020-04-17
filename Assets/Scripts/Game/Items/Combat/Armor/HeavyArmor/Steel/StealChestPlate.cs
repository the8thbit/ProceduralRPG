using UnityEngine;
using UnityEditor;

public class StealChestPlate : HeavyArmor
{
    public override int ID => Item.STEEL_CHEST_PLATE;
    public override string Name => "Steel Chest Plate";

    public override bool IsEquiptable => true;

    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.chest;
    public override int BaseProtection => 10;
    public override string SpriteTag => "test";

}