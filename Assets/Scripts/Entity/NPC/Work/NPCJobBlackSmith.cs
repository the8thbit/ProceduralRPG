using UnityEngine;
using UnityEditor;
[System.Serializable]
public class NPCJobBlackSmith : NPCJob
{
    private Blacksmith BlackSmith;
    public NPCJobBlackSmith(IWorkBuilding workLocation) : base("Blacksmith", workLocation)
    {
        BlackSmith = workLocation as Blacksmith;
    }
    public override Color GetShirtColor => Color.green;


}