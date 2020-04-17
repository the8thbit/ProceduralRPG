using UnityEngine;
using UnityEditor;

public abstract class Beam
{
    public abstract string ResourceCode { get; }
    public abstract GameObject GenerateBeamObject();
    public abstract void InternalOnCollision(Collider other);
    public abstract void Update();
    public abstract float MaxLength { get; }
}