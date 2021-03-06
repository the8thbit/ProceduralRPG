﻿using UnityEngine;
using UnityEditor;

public class NPCJobMarketStall : NPCJob
{
    public NPCJobMarketStall(string title, IWorkBuilding workLocation, WorldObjectData  marketStall, KingdomHierarchy rankReq = KingdomHierarchy.Citizen) : base(title, workLocation, rankReq)
    {
    }

    public override Color GetShirtColor => Color.white;

}