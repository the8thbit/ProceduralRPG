using UnityEngine;
using UnityEditor;
[System.Serializable]
public class Archery : Skill
{
    public Archery() : base(Skills.Archery)
    {
    }
    public override string Name => "Archery";

}