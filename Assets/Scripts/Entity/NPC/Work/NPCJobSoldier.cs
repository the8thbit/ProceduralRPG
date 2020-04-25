using UnityEngine;
using UnityEditor;

public class NPCJobSoldier : NPCJob
{

    public NPCJobSoldier(IWorkBuilding workLocation) : base("Soldier", workLocation, KingdomHierarchy.Citizen)
    {
    }

    public override Color GetShirtColor => Color.red;
}