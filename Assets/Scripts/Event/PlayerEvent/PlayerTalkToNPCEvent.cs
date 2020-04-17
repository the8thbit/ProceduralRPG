using UnityEngine;
using UnityEditor;

public interface IPlayerTalkToNPCEvent : IEventListener { void PlayerTalkToNPCEvent(PlayerTalkToNPC ev); }
public class PlayerTalkToNPC : IEvent
{
    public NPC NPC { get; private set; }
    public PlayerTalkToNPC(NPC npc)
    {
        NPC = npc;
    }
}