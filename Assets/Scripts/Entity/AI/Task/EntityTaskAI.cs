using UnityEngine;
using UnityEditor;
[System.Serializable]
public abstract class EntityTaskAI 
{
    protected Entity Entity;

    public void SetEntity(Entity e)
    {
        Entity = e;
    }

    public abstract void Update();
    public abstract void Tick();
}