using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Shirt : EquiptableItem
{
    public Shirt(ItemMetaData meta = null) : base(new ItemTag[] { ItemTag.ARMOR, ItemTag.ITEM}, meta)
    {
        if (MetaData == null)
        {
            MetaData = new ItemMetaData().SetColor(Color.yellow);
        }
    }


    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.chest;

    public override ItemID ID => ItemID.Shirt;

    public override string Name => "Shirt";

    public override string SpriteTag => "weapons";

    public override string SpriteSheetTag => "shirt";

    public override bool IsDefault => true;
}