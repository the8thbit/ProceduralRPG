using UnityEngine;
using UnityEditor;

[System.Serializable]
public abstract class Key : Item
{
    protected Key(int keyID) : base(new ItemTag[] { ItemTag.ITEM, ItemTag.KEY}, null)
    {
        KeyID = keyID;
    }


    public int KeyID { get; private set; }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        Key key = obj as Key;
        if (key == null)
            return false;
        return key.KeyID == KeyID;
    }
}