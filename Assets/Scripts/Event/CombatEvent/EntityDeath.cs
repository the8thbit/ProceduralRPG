using UnityEngine;
using UnityEditor;


public interface IEntityDeathListener : IEventListener
{
    void OnEntityDeathEvent(EntityDeath ede);
}
[System.Serializable]
public class EntityDeath : IEvent
{
    public Entity Entity { get; private set; }
    public EntityDeath(Entity dead)
    {
        Entity = dead;
    }
}