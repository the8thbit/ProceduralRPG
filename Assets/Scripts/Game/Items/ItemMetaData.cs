using UnityEngine;
using UnityEditor;

[System.Serializable]
public class ItemMetaData
{

    private float[] Color_;
    public Color Color { get { return Color_ == null ? Color.white : new Color(Color_[0], Color_[1], Color_[2]); } }
    public int KeyDungeonID;


   

    public ItemMetaData(int keyDungeonID=-1)
    {
        KeyDungeonID = keyDungeonID;
    }
    public ItemMetaData SetColor(Color color)
    {
        Color_ = new float[] { color.r, color.g, color.b };
        return this;
    }
    
}