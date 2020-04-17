using UnityEngine;
using UnityEditor;

public class NPCJobSoldier : NPCJob
{
    public NPCJobSoldier(WorkBuilding workLocation) : base("Soldier", workLocation, KingdomHierarchy.Citizen)
    {
    }


}