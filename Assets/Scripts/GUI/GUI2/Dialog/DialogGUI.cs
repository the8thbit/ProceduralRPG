using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogGUI : MonoBehaviour
{

    public bool InConversation { get; private set; }


    public GameObject PlayerReplyButtonPrefab;

    private NPC NPC;

    public Text NPCText;
    public GameObject PlayerRepliesRegion;
    public void SetNPC(NPC npc)
    {

        Debug.Log(PlayerRepliesRegion.GetComponent<RectTransform>().rect.width);

        NPC = npc;
        npc.Dialog.StartDialog();
        DisplayCurrentNode();
    }


    public void NodeReplyClicked(NPCDialogNode node)
    {
        //If the node is null (shouldn't be), or its an exit node, we clear the dialog GUI
        if(node == null || node.IsExitNode)
        {
            Clear();
            gameObject.SetActive(false);
            GameManager.PlayerManager.EndDialog();
        }
        else
        {
            //If the node is not an exit node, we call its selection function
            node.OnSelectNode();
            //And then Display the node
            NPC.Dialog.SetCurrentNode(node);
            DisplayCurrentNode();
        }
        
    }


    private void DisplayCurrentNode()
    {
        Clear();
        if(NPC.Dialog.CurrentNode == null)
        {
            NPCText.text = NPC.Dialog.DialogOpenText;
            foreach(NPCDialogNode node in NPC.Dialog.Nodes)
            {
                if(node.ShouldShow())
                    DisplayNode(node);
            }
        }
        else
        {
            NPCText.text = NPC.Dialog.CurrentNode.DialogText;
            foreach(NPCDialogNode node in NPC.Dialog.CurrentNode.ChildNodes)
            {
                if (node.ShouldShow())
                    DisplayNode(node);
            }
        }


    }

    private void DisplayNode(NPCDialogNode node)
    {
        GameObject nodeObj = Instantiate(PlayerReplyButtonPrefab);
        nodeObj.transform.SetParent(PlayerRepliesRegion.transform, false);
        DialogPlayerReplyButton but = nodeObj.GetComponent<DialogPlayerReplyButton>();
        but.InitButton(this, node);

    }

    private void Clear()
    {
        //Clear all old replies
        foreach (Transform t in PlayerRepliesRegion.transform)
        {
            Destroy(t.gameObject);
        }

    }

    private void OnDisable()
    {
        Clear();
        InConversation = false;
    }
    private void OnEnable()
    {
        InConversation = true;
    }
}
