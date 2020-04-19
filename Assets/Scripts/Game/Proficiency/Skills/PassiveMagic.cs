using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PassiveMagic : Skill
{
    public PassiveMagic() : base(Skills.PassiveMagic)
    {
    }

    public override string Name => "Passive Magic";

}