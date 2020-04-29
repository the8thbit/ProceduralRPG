using UnityEngine;
using UnityEditor;

public class ConsoleCommandSettlementData : ConsoleCommand
{
    public override string RunCommand(string[] args)
    {
        if(args == null || args.Length == 0)
        {
            return "Must specify settlement id";
        }
        if(args.Length == 1)
        {
            return ListAll(ParseSettlement(args[0]));
        }
        return "Un-known command";
    }

    private Settlement ParseSettlement(string arg)
    {
        //Try parsing an ID
        if (int.TryParse(arg, out int id))
        {
            return GameManager.WorldManager.World.GetSettlement(id);
        }
        return null;
    }

    private string ListAll(Settlement set)
    {
        if (set == null)
            return "Settlement null";


        int jobCount = 0;
        int workerCount = set.SettlementNPCIDs.Count;

        foreach(Building b in set.Buildings)
        {
            if(b is IWorkBuilding)
            {
                jobCount += (b as IWorkBuilding).GetWorkData.WorkCapacity;
            }
        }
        string set_ = "Settlement ";
        set_ += set.Name + "\n";
        set_ += "Total jobs: " + jobCount;
        set_ += "Total NPCs: " + workerCount;
        return set_;
    }
}