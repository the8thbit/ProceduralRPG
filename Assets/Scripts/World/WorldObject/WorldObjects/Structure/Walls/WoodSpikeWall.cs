using UnityEngine;
using UnityEditor;
[System.Serializable]
public class WoodSpikeWall : WorldObjectData
{


    public override WorldObjects ObjID => WorldObjects.WOOD_SPIKE;

    public override string Name => "Wood Spike";

    public WoodSpikeWall(Vec2i worldPosition) : base(worldPosition, null, null)
    {
    }
}