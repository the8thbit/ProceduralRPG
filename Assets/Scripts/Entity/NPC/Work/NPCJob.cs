﻿using UnityEngine;
using UnityEditor;

/// <summary>
/// Information about the place an NPC works
/// </summary>
/// 
[System.Serializable]
public abstract class NPCJob
{

    public abstract Color GetShirtColor { get; }

   
    public IWorkBuilding WorkLocation { get; private set; }
    public string Title { get; private set; }
    public KingdomHierarchy RequiredRank { get; private set; }
    public NPCJob(string title, IWorkBuilding workLocation, KingdomHierarchy rankReq = KingdomHierarchy.Citizen)
    {
        Title = title;
        WorkLocation = workLocation;
        RequiredRank = rankReq;

    }



}