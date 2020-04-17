using UnityEngine;
using UnityEditor;

/// <summary>
/// A large structure which holds an entrance to an elemental dungeon in the middle
/// </summary>
[System.Serializable]
public class ElementalDungeonStructure : ChunkStructure
{
    private Element Element;
    public ElementalDungeonStructure(Element element, Vec2i position, Vec2i size) : base(position, size)
    {
        Element = element;
    }
}