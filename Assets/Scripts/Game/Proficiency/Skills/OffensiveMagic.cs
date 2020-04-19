using UnityEngine;
using UnityEditor;
[System.Serializable]
public class OffensiveMagic : Skill
{
    public OffensiveMagic() : base(Skills.OffensiveMagic)
    {
    }
    public override string Name => "Offensive Magic";

}