using UnityEngine;
using UnityEditor;
[System.Serializable]
public class SimpleDungeonKey : Key
{
    public SimpleDungeonKey(int keyID) : base(keyID)
    {
    }

    public override ItemID ID => ItemID.SimpDungeonKey;

    public override string Name => "Simple dungeon key";


    public override string SpriteTag => "simp_key";

    public override string SpriteSheetTag => "weapons";
}