using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Torch : EquiptableItem
{
    public Torch(ItemMetaData meta = null) : base(new ItemTag[] { ItemTag.MISC, ItemTag.ITEM }, meta)
    {
    }

    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.offhand;

    public override bool IsDefault => false;

    public override ItemID ID => ItemID.Torch;

    public override string Name => "Torch";

    public override string SpriteTag => "Torch";

    public override string SpriteSheetTag => "weapons";
}