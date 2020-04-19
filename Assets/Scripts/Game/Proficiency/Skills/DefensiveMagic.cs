using UnityEngine;
using UnityEditor;
[System.Serializable]
public class DefensiveMagic : Skill
{
    public DefensiveMagic() : base(Skills.DefenciveMagic)
    {
    }
    public override string Name => "Defensive Magic";

}