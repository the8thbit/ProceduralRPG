using UnityEngine;
using UnityEditor;
[System.Serializable]
public class WorldObjectMetaData
{

    public static Vec2i[] ORIENTATIONS = {
        new Vec2i(0, 1), new Vec2i(1, 1), new Vec2i(1, 0),
        new Vec2i(1, -1), new Vec2i(0, -1), new Vec2i(0,0) };

    public Vec2i Direction;
    public float Height = 1;   

    public WorldObjectMetaData(Vec2i direction=null, float height = 1)
    {
        Direction = direction;
        Height = height;
    }

}