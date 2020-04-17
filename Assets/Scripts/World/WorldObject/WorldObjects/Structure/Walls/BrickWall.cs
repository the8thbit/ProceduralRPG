using UnityEngine;
using UnityEditor;
[System.Serializable]
public class BrickWall : WorldObjectData
{
    private int Height;
    public BrickWall(Vec2i worldPosition, int height=1, WorldObjectMetaData meta = null, Vec2i size = null) : base(worldPosition, meta, size)
    {
    }


    public override string Name => "brick_wall";

    public override WorldObjects ObjID => WorldObjects.WALL;

    public override WorldObject CreateWorldObject(Transform transform = null)
    {
        WorldObject obj = base.CreateWorldObject(transform);
        if (Height == 1)
        {

        }
        return obj;
    }

    public override void OnObjectLoad(WorldObject obj)
    {
        obj.GetComponentInChildren<MeshRenderer>().material = ResourceManager.GetMaterial("brick");
    }
}