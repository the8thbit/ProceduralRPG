using UnityEngine;
using UnityEditor;
[System.Serializable]
public abstract class WorkEquiptmentData : WorldObjectData
{
    public WorkEquiptmentData(Vec2i worldPosition, WorldObjectMetaData meta, Vec2i size = null) : base(worldPosition, meta, size)
    {
        userPosition = this.WorldPosition + new Vec2i(1, 1);
    }

    protected Vec2i userPosition;

    public virtual Vec2i UserPosition { get { return userPosition; } }

    public void SetUserTile(Vec2i pos)
    {
        userPosition = pos;
    }

    [System.NonSerialized]
    public NPC CurrentUser;

}