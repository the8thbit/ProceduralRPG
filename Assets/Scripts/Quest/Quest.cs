using UnityEngine;
using UnityEditor;

public class Quest
{
    public string QuestName { get; private set; }
    public QuestType QuestType { get; private set; }
    public QuestInitiator Initiator { get; private set; }
    public QuestTask[] Tasks { get; private set; }
    public bool Completed { get; private set; }
    private int _CurrentTask = -1;

    public Quest(string questName, QuestInitiator init, QuestTask[] tasks, QuestType questType)
    {
        QuestName = questName;
        Initiator = init;
        Tasks = tasks;
        QuestType = questType;
        Completed = false;
    
    }

    public void StartQuest()
    {
        _CurrentTask = 0;
    }
    public QuestTask NextTask()
    {
        _CurrentTask++;
        return CurrentTask();
    }
    
    public QuestTask CurrentTask()
    {
        if (_CurrentTask == -1)
            return null;
        if(_CurrentTask == Tasks.Length)
        {
            Completed = true;
            return null;
        }
        if (Completed)
            return null;
        if (!IsStarted())
            return null;
        return Tasks[_CurrentTask];
            
    }

    public bool IsStarted()
    {
        return _CurrentTask != -1;
    }
}

public enum QuestType
{
    clear_dungeon
}