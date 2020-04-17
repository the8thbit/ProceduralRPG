using UnityEngine;
using UnityEditor;
[System.Serializable]
public class NPCJobBlackSmith : NPCJob
{
    private Blacksmith BlackSmith;
    public NPCJobBlackSmith(WorkBuilding workLocation) : base("Blacksmith", workLocation)
    {
        BlackSmith = workLocation as Blacksmith;
    }


}