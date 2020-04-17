using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
[System.Serializable]
public class SettlementPathFinder
{

}
[System.Serializable]
public class SettlementPathNode
{

    public static Vec2i[] DIRECTIONS = new Vec2i[] {new Vec2i(0,1), new Vec2i(1,0), new Vec2i(0,-1), new Vec2i(-1,0) };

    public static int GetDirection(Vec2i v)
    {
        if(v.x != 0)
        {
            v.x = (int)Mathf.Sign(v.x);
        }if(v.z != 0)
        {
            v.z = (int)Mathf.Sign(v.z);
        }
        if(v.x != 0 && v.z != 0)
        {
            throw new System.Exception("Supplied vector has more than one component");
        }
        if (v.x == 1)
            return EAST;
        if (v.x == -1)
            return WEST;
        if (v.z == 1)
            return NORTH;
        return SOUTH;
    }

    public static Vec2i GetDirection(int direction)
    {
        return DIRECTIONS[direction];
    }
    public static Vec2i GetPerpendicular(int direction)
    {
        return GetDirection((direction + 1) % 4);
    }
    public static int GetPerpendicularDirection(int direction)
    {
        return (direction + 1) % 4;
    }

    public static int OppositeDirection(int direction)
    {
        return (direction + 2) % 4;
    }

    public static readonly int NORTH = 0;
    public static readonly int EAST = 1;
    public static readonly int SOUTH = 2;
    public static readonly int WEST = 3;


    public Vec2i Position { get; private set; }
    public SettlementPathNode[] Connected;
    public SettlementPathNode(Vec2i nodePosition)
    {
        Position = nodePosition;
        Connected = new SettlementPathNode[4];
    }

    public void AddConnection(int direction, SettlementPathNode node)
    {
        Connected[direction] = node;
    }

    public List<SettlementPathNode> GetConnected()
    {
        List<SettlementPathNode> connected = new List<SettlementPathNode>();
        foreach (SettlementPathNode con in Connected)
            if (con != null)
                connected.Add(con);
        return connected;
    }

    /// <summary>
    /// Checks if this node is connected to any other
    /// </summary>
    /// <returns></returns>
    public bool HasConnection()
    {
        foreach (SettlementPathNode connected in Connected)
            if (connected != null)
                return true;
        return false;
    }

}