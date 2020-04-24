using UnityEngine;
using UnityEditor;
[System.Serializable]
public class Trousers : EquiptableItem
{
    public Trousers(ItemMetaData meta = null) : base(new ItemTag[] { ItemTag.ARMOR, ItemTag.ITEM }, meta)
    {
        if (MetaData == null)
        {
            MetaData = new ItemMetaData().SetColor(Color.red);
        }
    }

    public override EquiptmentSlot EquiptableSlot => EquiptmentSlot.legs;

    public override bool IsDefault => true;

    public override ItemID ID => ItemID.Trousers;

    public override string Name => "Trousers";

    public override string SpriteTag => "weapons";

    public override string SpriteSheetTag => "Trousers";
}