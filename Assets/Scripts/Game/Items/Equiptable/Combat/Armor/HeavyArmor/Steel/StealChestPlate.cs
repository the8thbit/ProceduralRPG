using UnityEngine;
using UnityEditor;

public class StealChestPlate : HeavyArmor
{
    public override ItemID ID => ItemID.SteelChest;
    public override string Name => "Steel Chest Plate";


    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.chest;
    public override int BaseProtection => 10;
    public override string SpriteTag => "test";

}