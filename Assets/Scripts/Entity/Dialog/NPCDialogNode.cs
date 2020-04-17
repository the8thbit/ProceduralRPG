using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[System.Serializable]
public class NPCDialogNode
{
    private NPCDialog Parent;


    public string PlayerText { get; private set; }
    public string DialogText { get; private set; }

    public List<NPCDialogNode> ChildNodes;

    public delegate void OnSelectNodeFunction();
    public delegate bool ShouldDisplayNodeFunction();

    private Delegate ShouldDisplayNodeFunction_;
    private Delegate OnSelectNodeFunction_;
    public NPCDialogNode(string playerText, string dialogText)
    {
        PlayerText = playerText;
        DialogText = dialogText;
        ChildNodes = new List<NPCDialogNode>();
    }

    public bool IsExitNode { get; set; }


    public void SetOnSelectFunction(OnSelectNodeFunction fun)
    {
        OnSelectNodeFunction_ = fun;
    }
    public void SetShouldDisplayFunction(ShouldDisplayNodeFunction fun)
    {
        ShouldDisplayNodeFunction_ = fun;
    }


    public void AddNode(NPCDialogNode node)
    {
        ChildNodes.Add(node);
        node.SetParent(Parent);
    }

    public void OnSelectNode()
    {
        OnSelectNodeFunction_?.DynamicInvoke();
        Debug.Log("node selected");
    }

    public bool ShouldShow()
    {
        if (ShouldDisplayNodeFunction_ != null)
            return (bool)ShouldDisplayNodeFunction_.DynamicInvoke();
        return true;
    }
    public void SetParent(NPCDialog dialog)
    {
        Parent = dialog;
    }
}