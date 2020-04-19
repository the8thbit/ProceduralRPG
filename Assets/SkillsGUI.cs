using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillsGUI : MonoBehaviour
{

    public Text Text;

    /// <summary>
    /// Sets the <see cref="Text"/> text to display the players
    /// skill tree
    /// </summary>
    private void OnEnable()
    {
        //Generate and set skill tree
        Text.text = GenerateSkillTreeText(GameManager.PlayerManager.Player.SkillTree);
    }


    /// <summary>
    /// Generates a simple text output containing the total XP and 
    /// level for each skill in the given skilltree
    /// </summary>
    /// <param name="skilltree"></param>
    /// <returns></returns>
    private string GenerateSkillTreeText(SkillTree skilltree)
    {
        //Init empty string for skill tree
        string st = "";

        //Iterate each skill
        foreach(Skills skill in MiscUtils.GetValues<Skills>())
        {
            //Get the relevent Skill object from the skill tree
            Skill curSkill =skilltree.GetSkill(skill);
            //Create skill information
            st += "<b>" + curSkill.Name + "</b>\n";
            st += "    Current XP: " + curSkill.GetXP() + "\n";
            st += "    Current Level: " + curSkill.GetLevel() + "\n\n";

        }

        return st;


    }
}
