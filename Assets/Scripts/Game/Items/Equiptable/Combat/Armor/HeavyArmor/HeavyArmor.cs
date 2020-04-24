using UnityEngine;
using UnityEditor;
[System.Serializable]
public abstract class HeavyArmor : Armor
{

    public HeavyArmor(ItemMetaData meta = null) : base(meta)
    {
    }
    public override string SpriteSheetTag => "armour"; //TODO - Change (test only)
    
}