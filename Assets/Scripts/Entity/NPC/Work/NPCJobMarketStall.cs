using UnityEngine;
using UnityEditor;

public class NPCJobMarketStall : NPCJob
{
    public NPCJobMarketStall(string title, WorkBuilding workLocation, WorldObject marketStall, KingdomHierarchy rankReq = KingdomHierarchy.Citizen) : base(title, workLocation, rankReq)
    {
    }


}