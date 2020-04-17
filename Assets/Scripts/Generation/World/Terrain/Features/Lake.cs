using UnityEngine;
using UnityEditor;

public class Lake : ChunkGenerationFeature
{

    public Vec2i Centre { get; private set; }
    public float Radius { get; private set; }
    public Lake(Vec2i centre, float lakeRadius)
    {
        Centre = centre;
        Radius = lakeRadius;
    }

}