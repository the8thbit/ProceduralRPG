using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour, IPointerClickHandler
{

    private QuestGUI Parent;
    private Quest Quest;
    public Text Text;


    public void SetQuestButton(QuestGUI parent, Quest quest, bool inProgress=false)
    {
        Parent = parent;
        Quest = quest;
        if (inProgress)
        {

        }
    }

    void OnLeftClick()
    {
        Debug.Log("quest left");
        Parent.SetSelectedQuest(Quest);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }

    }
}