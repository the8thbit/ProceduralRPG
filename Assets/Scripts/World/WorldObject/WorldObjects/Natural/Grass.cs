using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Grass : WorldObjectData
{
    public Grass(Vec2i worldPosition) : base(worldPosition, null, null)
    {
    }


    public override void OnObjectLoad(WorldObject obj)
    {
        obj.transform.localScale = new Vector3(1.5f, 2f, 1.5f);
    }

    public override WorldObjects ObjID => WorldObjects.GRASS;



    public override string Name => "grass";
}