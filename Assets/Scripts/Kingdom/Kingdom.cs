using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 
/// </summary>
/// 

[System.Serializable]
public class Kingdom
{


    public string Name { get; private set; } //The name of this kingdom
    public int KingdomID { get; private set; }

    public Vec2i CapitalChunk { get; private set; } //The chunk the capital is centred on.
    
    public List<Vec2i> ClaimedChunks { get; private set; }
    public List<Settlement> Settlements { get; private set; }

    public List<int> SettlementIDs;
    public Kingdom(string name, Vec2i capitalChunk)
    {
        Name = name;
        CapitalChunk = capitalChunk;

        ClaimedChunks = new List<Vec2i>();
        Settlements = new List<Settlement>();
        SettlementIDs = new List<int>();
    }

    public void SetKingdomID(int id)
    {
        KingdomID = id;
    }

    /// <summary>
    /// Removes any duplicates
    /// </summary>
    public void CleanupTerritory()
    {
        ClaimedChunks = this.ClaimedChunks.Distinct<Vec2i>().ToList<Vec2i>();

    }

    public void AddSettlement(Settlement set)
    {
        SettlementIDs.Add(set.SettlementID);
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (!(obj is Kingdom))
            return false;
        return CapitalChunk == (obj as Kingdom).CapitalChunk;
        return base.Equals(obj);
    }

}