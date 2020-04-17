using UnityEngine;
using UnityEditor;
[System.Serializable]
public class BuildingEntrance
{
    public Vec2i A { get; private set; }
    public Vec2i B { get; private set; }
    public BuildingEntrance(Vec2i a, Vec2i b)
    {
        A = a;
        B = b;
    }
}