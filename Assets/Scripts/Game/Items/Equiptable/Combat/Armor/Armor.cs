using UnityEngine;
using UnityEditor;
[System.Serializable]
public abstract class Armor : EquiptableItem
{
    public static readonly ItemTag[] ARMOR_ITEM_TAGS = new ItemTag[] { ItemTag.ARMOR, ItemTag.ITEM };

    protected Armor(ItemMetaData meta = null) : base(ARMOR_ITEM_TAGS, meta)
    {
    }
    public abstract int BaseProtection { get; }
    public override bool IsDefault => false;
}