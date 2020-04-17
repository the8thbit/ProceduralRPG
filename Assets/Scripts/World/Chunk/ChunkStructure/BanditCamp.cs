using UnityEngine;
using UnityEditor;
[System.Serializable]
public class BanditCamp : ChunkStructure
{

    public int BanditCampLevel { get { return Size.x * Size.z; } }

    public BanditCamp(Vec2i position, Vec2i size) : base(position, size)
    {
    }
}