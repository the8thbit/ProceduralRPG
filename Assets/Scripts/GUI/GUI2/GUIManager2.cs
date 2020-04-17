using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager2 : MonoBehaviour
{
    public static GUIManager2 Instance;

    public IngameGUI IngameGUI;
    public OpenGUI OpenGUI;
    public DialogGUI DialogGUI;
    public bool OpenGUIVisible { get; private set; }

    public Button[] Buttons;


    void OnEnable()
    {
        Instance = this;
        OpenGUIVisible = false;
        OpenGUI.gameObject.SetActive(false);
        DialogGUI.gameObject.SetActive(false);
    }


    public void SetVisible(bool vis)
    {
        OpenGUIVisible = vis;
        OpenGUI.gameObject.SetActive(vis);

    }

    public void StartDialog(NPC npc)
    {
        DialogGUI.gameObject.SetActive(true);
        DialogGUI.SetNPC(npc);
    }



}
