using UnityEngine;
using UnityEditor;
/// <summary>
/// This defines the listener for this event
/// </summary>
public interface IPlayerPickupItemEvent : IEventListener { void PlayerPickupItemEvent(PlayerPickupItem ev); }
public class PlayerPickupItem : IEvent
{
    public Item Item { get; private set; }
    public PlayerPickupItem(Item item)
    {
        Item = item;
    }
}

public interface IPlayerDropItemEvent : IEventListener { void PlayerDropItemEvent(PlayerDropItem ev); }
public class PlayerDropItem : IEvent
{
    public Item Item { get; private set; }
    public PlayerDropItem(Item item)
    {
        Item = item;
    }
}