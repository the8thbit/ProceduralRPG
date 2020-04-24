using UnityEngine;
using UnityEditor;

[System.Serializable]
public abstract class EquiptableItem : Item
{
    protected EquiptableItem(ItemTag[] tags, ItemMetaData meta = null) : base(tags, meta)
    {
    }

    public abstract EquiptmentSlot EquiptableSlot { get; }

    public GameObject GetEquiptItem()
    {
        return ResourceManager.GetEquiptableItemObject(ID);
    }
    public abstract bool IsDefault { get; }
}