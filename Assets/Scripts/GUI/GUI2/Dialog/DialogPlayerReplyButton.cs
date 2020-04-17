using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class DialogPlayerReplyButton : MonoBehaviour, IPointerClickHandler
{

    public int MaxWidth = 500;

    public Text Text;
    private DialogGUI Parent;
    private NPCDialogNode Node;
    public void InitButton(DialogGUI parent, NPCDialogNode node)
    {
        Text.text = node.PlayerText;
        Parent = parent;
        Node = node;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Parent.NodeReplyClicked(Node);
        }
    }
}