using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IngameGUI : MonoBehaviour
{
    public Text NPCText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetNPC(NPC npc)
    {
        if(npc == null)
        {
            NPCText.text = "";
            NPCText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            NPCText.transform.parent.gameObject.SetActive(true);
            NPCText.text = "";
            NPCText.text += npc.Name + "\n";
            NPCText.text += npc.EntityRelationshipManager.Personality.ToString();
        }
    }
}
