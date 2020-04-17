using UnityEngine;
using UnityEditor;
[System.Serializable]
public abstract class HeavyArmor : Armor
{

    public HeavyArmor(ItemMetaData meta = null) : base(meta)
    {
    }
    public override bool IsEquiptable => true;
    public override string SpriteSheetTag => "weapon"; //TODO - Change (test only)
}