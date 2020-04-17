using UnityEngine;
using UnityEditor;

public class DamageRegionArgKockback
{
    public Vector2 Direction { get; private set; }
    public float Force { get; private set;  }
    public DamageRegionArgKockback(Vector2 direction, float force)
    {
        Direction = direction;
        Force = force;
    }
}