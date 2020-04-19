using UnityEngine;
using UnityEditor;
[System.Serializable]
public class HeavyArmour : Skill
{
    public HeavyArmour() : base(Skills.HeavyArmour)
    {
    }

    public override string Name => "Heavy Armour";
}