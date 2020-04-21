using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGUI : MonoBehaviour
{
    public static DebugGUI Instance;
    private Dictionary<string, string> DebugLines;
    private LoadedEntity SelectedEntity;
    void Awake()
    {
        Instance = this;
        DebugLines = new Dictionary<string, string>();
    }


    public void SetData(string key, object obj)
    {
        SetData(key, obj == null ? "null" : obj.ToString());
    }
    public void SetData(string key, string value)
    {
        if (!DebugLines.ContainsKey(key))
        {
            DebugLines.Add(key, value);
        }
        else
        {
            DebugLines[key] = value;
        }
    }

    public void SetSelectedEntity(LoadedEntity e)
    {

        SelectedEntity = e;
    }

    public void GUIDRAWLINE(Vector3 start, Vector3 end)
    {

    }
    private void OnGUI()
    {
        //GUI.DrawTexture(new Rect(0, 0, Screen.height, Screen.height), WorldMap);

        int i = 0;
        Vector2 xy = new Vector2(40, 40); //Start pos top left
        Vector2 lineDiff = new Vector2(0, 20);
        Vector2 line = new Vector2(800, 20);
        GUI.color = Color.black;
        foreach(KeyValuePair<string,string> kpv in DebugLines)
        {
            GUI.Label(new Rect(xy + lineDiff * i, line), new GUIContent(kpv.Key + " - " + kpv.Value));
            i++;
        }

        if(SelectedEntity != null)
        {
            
            string[] entDebug = SelectedEntity.Entity.EntityDebugData();
            int len = entDebug.Length;
            float linHi = 25;
            string debug = "";
            foreach(string s in entDebug)
            {
                debug += s + "\n";
            }


            GUI.Box(new Rect(200, 200, 600, linHi * len), debug);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetData("fps", (1f / Time.deltaTime).ToString());
    }
    
}
