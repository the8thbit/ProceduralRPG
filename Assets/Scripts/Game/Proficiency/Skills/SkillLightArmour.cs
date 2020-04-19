using UnityEngine;
using UnityEditor;
[System.Serializable]
public class SkillLightArmour : Skill
{
    public SkillLightArmour() : base(Skills.LightArmour)
    {
    }

    public override string Name => "Light Armour";

}