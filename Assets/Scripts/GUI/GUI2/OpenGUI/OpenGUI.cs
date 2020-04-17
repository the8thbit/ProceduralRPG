using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenGUI : MonoBehaviour
{

    public static readonly int EQUIPTMENT = 0;
    public static readonly int INVENTORY = 1;
    public static readonly int SPELLS = 2;
    public static readonly int SKILLS = 3;
    public static readonly int QUESTS = 4;
    public static readonly int WORLDMAP = 5;


    public GameObject[] Panels;
    private int CurrentButton = 0;

    private void OnEnable()
    {
        Panels[CurrentButton].SetActive(true);
        GameManager.SetPause(true);
    }
    /// <summary>
    /// Called when a main openGUI button is pressed.
    /// Changes the currently visible GUI
    /// </summary>
    /// <param name="button"></param>
    public void OnButtonPress(int button)
    {

        if (button == CurrentButton)
            return;

        Panels[CurrentButton].SetActive(false);
        Panels[button].SetActive(true);

        CurrentButton = button;

    }

    private void OnDisable()
    {
        GameManager.SetPause(false);
    }

}
