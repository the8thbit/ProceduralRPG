using UnityEngine;
using UnityEditor;
[System.Serializable]
public class SimpleDungeonKey : Key
{
    public SimpleDungeonKey(int keyID) : base(keyID)
    {
    }

    public override int ID => Item.SIMPLE_DUNGEON_KEY;

    public override string Name => "Simple dungeon key";

    public override bool IsEquiptable => false;

    public override EquiptmentSlot EquiptableSlot => throw new System.NotImplementedException();

    public override string SpriteTag => "simp_key";

    public override string SpriteSheetTag => "weapons";
}