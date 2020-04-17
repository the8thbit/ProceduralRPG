using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestGUI : MonoBehaviour
{

    public GameObject QuestButtonPrefab;

    public GameObject AllQuestViewport;
    public Text QuestDetailsText;
    private QuestManager QuestManager;

    public Quest SelectedQuest { get; private set; }

    private void OnEnable()
    {
        QuestManager = GameManager.QuestManager;
        foreach(Quest q in QuestManager.InProgress)
        {
            GameObject questBut = Instantiate(QuestButtonPrefab);
            questBut.transform.SetParent(AllQuestViewport.transform, false);
            questBut.GetComponent<QuestButton>().SetQuestButton(this, q);
        }
        foreach(Quest q in QuestManager.Unstarted)
        {
            GameObject questBut = Instantiate(QuestButtonPrefab);
            questBut.transform.SetParent(AllQuestViewport.transform, false);
            questBut.GetComponent<QuestButton>().SetQuestButton(this, q);
        }

        SetSelectedQuest(QuestManager.Unstarted[0]);


    }


    

    public void OnButtonPress(int button)
    {
        
    }

    public void SetSelectedQuest(Quest quest)
    {
        SelectedQuest = quest;
        if (SelectedQuest == null)
        {
            QuestDetailsText.text = "";
        }
        else
        {
            QuestDetailsText.text = GenerateQuestDetails(quest);
        }
    }

    private string GenerateQuestDetails(Quest quest)
    {
        string total = quest.QuestName;

        total += "\n" + quest.Initiator.ToString();

        foreach (QuestTask task in quest.Tasks)
        {
            if (task == quest.CurrentTask())
            {
                total += "\n<b>" + task.ToString() + "</b>";
            }
            else
            {
                total += "\n" + task.ToString();
            }
            
        }

        return total;
    }

    private void OnDisable()
    {
        foreach (Transform t in AllQuestViewport.transform)
        {
            Destroy(t.gameObject);
        }
    }

}
