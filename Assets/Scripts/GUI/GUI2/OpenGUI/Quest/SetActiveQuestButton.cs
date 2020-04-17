using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SetActiveQuestButton : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameManager.QuestManager.SetActiveQuest(GameManager.GUIManager.OpenGUI.Panels[OpenGUI.QUESTS].GetComponent<QuestGUI>().SelectedQuest);
        }

    }

}
