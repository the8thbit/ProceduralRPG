using UnityEngine;
using UnityEditor;

[System.Serializable]
public class ItemMetaData
{



    public int KeyDungeonID;
    public ItemMetaData(int keyDungeonID=-1)
    {
        KeyDungeonID = keyDungeonID;
    }
    
}