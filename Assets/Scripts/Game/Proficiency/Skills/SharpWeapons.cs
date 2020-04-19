using UnityEngine;
using UnityEditor;
[System.Serializable]
public class SharpWeapons : Skill
{
    public SharpWeapons() : base(Skills.SharpWeapons)
    {
    }

    public override string Name => "Sharp Weapons";

}