using UnityEngine;
using UnityEditor;

/// <summary>
/// An empty interface that is extended by any event, such that it can easily be added within the event manager
/// </summary>
public interface IEventListener { }
public interface IEvent { }
public class EventManager 
{

    public static EventManager Instance;
    public EventManager()
    {
        Instance = this;
    }

    public delegate void PlayerPickupItemEvent(PlayerPickupItem ev);
    public event PlayerPickupItemEvent PlayerPickupItemEvent_;

    public delegate void PlayerDropItemEvent(PlayerDropItem ev);
    public event PlayerDropItemEvent PlayerDropItemEvent_;

    public delegate void PlayerTalkToNPCEvent(PlayerTalkToNPC ev);
    public event PlayerTalkToNPCEvent PlayerTalkToNPCEvent_;

    public delegate void WorldDamageRegionEvent(NewDamageRegion ev);
    public event WorldDamageRegionEvent WorldDamageRegionEvent_;

    public delegate void GamePauseEvent(bool pause);
    public event GamePauseEvent GamePauseEvent_;

    public delegate void WorldCombatEvent(WorldCombat ev);
    public event WorldCombatEvent WorldCombatEvent_;

    public delegate void EntityDeathEvent(EntityDeath ev);
    public event EntityDeathEvent EntityDeathEvent_;

    /// <summary>
    /// Takes the given EventListener argument and see what type of listener it is.
    /// It then adds the listener to all relevent event call functions.
    /// </summary>
    /// <param name="list"></param>
    public void AddListener(IEventListener list)
    {
        if (list is IPlayerPickupItemEvent)
            PlayerPickupItemEvent_ += (list as IPlayerPickupItemEvent).PlayerPickupItemEvent;
        if (list is IPlayerDropItemEvent)
            PlayerDropItemEvent_ += (list as IPlayerDropItemEvent).PlayerDropItemEvent;
        if (list is IPlayerTalkToNPCEvent)
            PlayerTalkToNPCEvent_ += (list as IPlayerTalkToNPCEvent).PlayerTalkToNPCEvent;
        if(list is INewDamageRegionEvent)        
            WorldDamageRegionEvent_ += (list as INewDamageRegionEvent).NewDamageRegionEvent;
        if(list is IGamePauseEvent)        
            GamePauseEvent_ += (list as IGamePauseEvent).GamePauseEvent;
        if (list is IWorldCombatEvent)
            WorldCombatEvent_ += (list as IWorldCombatEvent).WorldCombatEvent;
        if(list is IEntityDeathListener)
        {
            EntityDeathEvent_ += (list as IEntityDeathListener).OnEntityDeathEvent;
        }

    }

    /// <summary>
    /// Checks the instance of event and removes from the event call function
    /// </summary>
    /// <param name="list"></param>
    public void RemoveListener(IEventListener list)
    {
        if (list is IPlayerPickupItemEvent)
            PlayerPickupItemEvent_ -= (list as IPlayerPickupItemEvent).PlayerPickupItemEvent;
        if (list is IPlayerDropItemEvent)
            PlayerDropItemEvent_ -= (list as IPlayerDropItemEvent).PlayerDropItemEvent;
        if (list is IPlayerTalkToNPCEvent)
            PlayerTalkToNPCEvent_ -= (list as IPlayerTalkToNPCEvent).PlayerTalkToNPCEvent;
        if (list is INewDamageRegionEvent)
            WorldDamageRegionEvent_ -= (list as INewDamageRegionEvent).NewDamageRegionEvent;
        if (list is IGamePauseEvent)
            GamePauseEvent_ -= (list as IGamePauseEvent).GamePauseEvent;
        if (list is IWorldCombatEvent)
            WorldCombatEvent_ -= (list as IWorldCombatEvent).WorldCombatEvent;
        if (list is IEntityDeathListener)
        {
            EntityDeathEvent_ -= (list as IEntityDeathListener).OnEntityDeathEvent;
        }
    }

    /// <summary>
    /// Checks what event this instance is of, and invokes relevent events.
    /// </summary>
    /// <param name="ev"></param>
    public void InvokeNewEvent(IEvent ev){
        if (ev is PlayerPickupItem)
            PlayerPickupItemEvent_?.Invoke(ev as PlayerPickupItem);
        if (ev is PlayerDropItem)
            PlayerDropItemEvent_?.Invoke(ev as PlayerDropItem);
        if (ev is PlayerTalkToNPC)
            PlayerTalkToNPCEvent_?.Invoke(ev as PlayerTalkToNPC);
        if(ev is NewDamageRegion)
        {
            WorldDamageRegionEvent_?.Invoke(ev as NewDamageRegion);
        }
        if(ev is GamePause)
        {
            GamePauseEvent_?.Invoke((ev as GamePause).Pause);
        }
        if (ev is WorldCombat)
            WorldCombatEvent_?.Invoke(ev as WorldCombat);

        if(ev is EntityDeath)
        {
            EntityDeathEvent_?.Invoke(ev as EntityDeath);
        }

    }

}