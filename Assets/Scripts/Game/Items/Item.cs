using UnityEngine;
using UnityEditor;
[System.Serializable]
public abstract class Item
{
    public static int STEEL_CHEST_PLATE = 0, STEEL_HELM = 1, STEEL_LEGS = 2, STEEL_BOOTS = 3, STEEL_GLOVES = 4,
        STEEL_LONG_SWORD = 5, SHORT_BOW = 6, SIMPLE_DUNGEON_KEY=7;

    public abstract ItemID ID { get; }
    public abstract string Name { get; }
    public abstract string SpriteTag { get; }
    public abstract string SpriteSheetTag { get; }
    public ItemTag[] Tags { get; private set; }
    public ItemMetaData MetaData { get; protected set; }
    public bool HasMetaData { get { return MetaData != null; } }

    protected Item(ItemTag[] tags, ItemMetaData meta=null)
    {
        Tags = tags;
        MetaData = meta;
    }

    public void SetMetaData(ItemMetaData meta)
    {
        MetaData = meta;
    }


  
    public Sprite GetItemImage()
    {
        return ResourceManager.GetItemSprite(SpriteSheetTag, SpriteTag);
    }


    public bool HasTag(ItemTag tag)
    {
        return System.Array.IndexOf(Tags, tag) > -1;
    }
    public override bool Equals(object obj)
    {
        //Check first for null, then check if given object is an item. If false, return false
        if (obj == null)
            return false;
        Item b = obj as Item;
        if (b == null)
            return false;

        //If items have different ID, then they are different items
        if (ID != b.ID)
            return false;
        //If this item has meta data, then check if our meta data is equal to that of the given object
        if (HasMetaData)
        {
            //If the other object has no meta data, this value will be null and so will return false as required.
            return MetaData.Equals(b.MetaData); 
        }
        //We have established that this Item has no meta data, and so if the other item has meta data the items are not equivilent
        if (!b.HasMetaData)
            return false;
        return b.MetaData.Equals(MetaData);
    }


}

public enum ItemID
{
    Shirt, Trousers,
    Torch,
    SteelChest, SteelLegs, SteelHelm, SteelBoots,SteelGauntlets,
    SteelLongSword, 
    ShortBow, 
    SimpDungeonKey
}