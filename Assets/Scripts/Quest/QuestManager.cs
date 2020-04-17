using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;
/// <summary>
/// Holds onto all quests for the game
/// </summary>
public class QuestManager : MonoBehaviour, IPlayerTalkToNPCEvent, IPlayerPickupItemEvent, IEntityDeathListener
{


    public Quest ActiveQuest { get; private set; }
    public List<Quest> Unstarted { get; private set; }
    public List<Quest> InProgress { get; private set; }
    public List<Quest> Completed { get; private set; }

    private float Timer;


    public void SetActiveQuest(Quest quest)
    {
        ActiveQuest = quest;
        if (ActiveQuest != null)
            Debug.Log(ActiveQuest.QuestName);
    }

    /// <summary>
    /// Incriments timer to run a slow tick oncer per second
    /// <see cref="SlowTickUpdate"/>
    /// </summary>
    void Update()
    {
        Timer += Time.deltaTime;
        if(Timer > 1)
        {
            SlowTickUpdate();
        }
    }
    /// <summary>
    /// Slow tick on quests finds the players chunk, and uses this to check for 
    /// 'Go to Location' quest requirements. 
    /// </summary>
    private void SlowTickUpdate()
    {
        //Find the players position
        Vec2i pChunkPos = World.GetChunkPosition(GameManager.PlayerManager.Player.Position);
        List<Quest> taskComplete = new List<Quest>();

        foreach (Quest q in InProgress)
        {
            if(q.CurrentTask().TaskType == QuestTask.QuestTaskType.GO_TO_LOCATION)
            {
                WorldMapLocation wml = q.CurrentTask().GetLocation();
                if (wml.ChunkPosition == pChunkPos)
                {
                    taskComplete.Add(q);
                }
            }
        }
        foreach(Quest q in taskComplete)
        {
            QuestCurrentTaskComplete(q);
        }
    }



    public void SetQuests(List<Quest> quests)
    {
        GameManager.EventManager.AddListener(this);
        Unstarted = quests;
        InProgress = new List<Quest>();
        Completed = new List<Quest>();
    }

   
    public void StartQuest(Quest quest)
    {
        Unstarted.Remove(quest);
        InProgress.Add(quest);
        quest.StartQuest();
        Debug.Log("Quest " + quest.QuestName + " has started");
        Debug.Log("Next task " + quest.CurrentTask());
    }
    public void QuestCurrentTaskComplete(Quest quest)
    {
        QuestTask l = quest.CurrentTask();
        QuestTask t = quest.NextTask();

        Debug.Log("Current: " + l.ToString() + " _ " + t);

        if(t==null && quest.Completed)
        {
            Debug.Log("Quest " + quest.QuestName + " is completed!!");
            InProgress.Remove(quest);
            Completed.Add(quest);
            if (ActiveQuest == quest)
                ActiveQuest = null;
        }
        else
        {
            Debug.Log("Task \'" + l.Description + "\' for quest " + quest.QuestName + " is complete. Next, " + t.Description);
        }
    }
    public bool QuestTaskComplete(Quest quest, QuestTask task)
    {
        if (quest == null)
            return false;
        if (!quest.IsStarted())
            return false;
        if (quest.CurrentTask() != task)
            return false;
        QuestCurrentTaskComplete(quest);
        return true;
    }

    #region event_listeners
    public void PlayerTalkToNPCEvent(PlayerTalkToNPC ev)
    {
        List<Quest> toStart = new List<Quest>();
        //Iterate all unstarted quests
        foreach (Quest q in Unstarted)
        {
            //If the quest is initiated by talking to an NPC, we check if this NPC is the relvent one
            if (q.Initiator.InitType == QuestInitiator.InitiationType.TALK_TO_NPC && q.Initiator.GetNPC() == ev.NPC && q.Initiator.GetArg<bool>(1))
            {
                //If so, we add to the list of quests we need to start
                toStart.Add(q);
            }
        }
        foreach (Quest q in toStart)
        {
            StartQuest(q);
        }

        List<Quest> taskComplete = new List<Quest>();

        foreach (Quest q in InProgress)
        {
            if (q.CurrentTask().TaskType == QuestTask.QuestTaskType.TALK_TO_NPC && q.CurrentTask().GetNPC() == ev.NPC)
            {
                taskComplete.Add(q);
            }
        }

        foreach (Quest q in taskComplete)
        {
            QuestCurrentTaskComplete(q);
        }
    }

    
    public void PlayerPickupItemEvent(PlayerPickupItem ev)
    {


        List<Quest> toStart = new List<Quest>();
        Item item = ev.Item;
        foreach (Quest q in Unstarted)
        {
            //If the quest is initiated by talking to an NPC, we check if this NPC is the relvent one
            if (q.Initiator.InitType == QuestInitiator.InitiationType.TALK_TO_NPC && item.Equals(q.Initiator.GetItem()))
            {
                //If so, we add to the list of quests we need to start
                toStart.Add(q);
            }
        }
        foreach (Quest q in toStart)
        {
            StartQuest(q);
        }
        List<Quest> taskComplete = new List<Quest>();

        foreach (Quest q in InProgress)
        {

            
            if (q.CurrentTask().TaskType == QuestTask.QuestTaskType.PICK_UP_ITEM && item.Equals(q.CurrentTask().GetItem()))
            {

                taskComplete.Add(q);
            }
        }

        foreach (Quest q in taskComplete)
        {
            QuestCurrentTaskComplete(q);
        }

    }

    public void OnEntityDeathEvent(EntityDeath ede)
    {
        Debug.Log("death");
        List<Quest> taskComplete = new List<Quest>();

        foreach (Quest q in InProgress)
        {
            if (q.CurrentTask().TaskType == QuestTask.QuestTaskType.KILL_ENTITY)
                Debug.Log(ede.Entity + "_" + q.CurrentTask().GetEntity());
            if (q.CurrentTask().TaskType == QuestTask.QuestTaskType.KILL_ENTITY && ede.Entity.Equals(q.CurrentTask().GetEntity()))
            {

                taskComplete.Add(q);
            }
        }

        foreach (Quest q in taskComplete)
        {
            QuestCurrentTaskComplete(q);
        }
    }
    #endregion
}