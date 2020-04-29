using UnityEngine;
using UnityEditor;

public class PractiseDummy : WorkEquiptmentData
{
    public PractiseDummy(Vec2i worldPosition, WorldObjectMetaData meta, Vec2i size = null) : base(worldPosition, meta, size)
    {
    }

    public override WorldObjects ObjID => WorldObjects.PRACTISE_DUMMY;

    public override string Name => "Practise Dummy";
}