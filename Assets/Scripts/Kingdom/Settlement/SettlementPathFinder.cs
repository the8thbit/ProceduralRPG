using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
[System.Serializable]
public class SettlementPathFinder
{

    public static bool SettlementPath(SettlementPathNode start, SettlementPathNode end, out List<SettlementPathNode> path, int maxTest = 50, bool debug=false)
    {


        List<SettlementPathNode> result = new List<SettlementPathNode>();

        List<SettlementPathNode> tested = new List<SettlementPathNode>(32);

        if (debug)
        {
            Debug.Log("[SetPathFind] Attempting path: " + start + " -> " + end);
        }

        bool found = false;
        SettlementPathNode current = start;
        result.Add(current);
        int index = 0;
        while (!found)
        {
            index++;

            if(index > maxTest)
            {
                path = result;
                return false;
            }
            int[] connectedVals = new int[4];
            int minVal = -1;
            int minNode = -1;
            if (debug)
            {
                Debug.Log("[SetPathFind] Current node: " + current.Position);
            }
            //Iterate 
            for (int i=0; i<4; i++)
            {

                if (debug)
                {
                    Debug.Log("[SetPathFind] Checking node: " + i);
                    if(current.Connected[i] != null)
                    {
                        Debug.Log("[SetPathFind] Not null: " + current.Connected[i]);

                    }
                }

                //If the node is null, then the value is max
               
                if (current.Connected[i] == null)
                {

                    if(debug)
                        Debug.Log("[SetPathFind] Node null -> inf val");
                    connectedVals[i] = int.MaxValue;

                }
                //If the connected node is our final node, we finish the algorithm
                else if(current.Connected[i].Position == end.Position)
                {
                    if (debug)
                        Debug.Log("[SetPathFind] Connected node is end");

                    result.Add(current.Connected[i]);
                    path = result;
                    return true;
                }
                else if(current.Connected[i].ConnectedCount < 2)
                { //If the connected node has less than two connections, it is an end node, and so goes no where
                    //We can set the connected value as high
                    if (debug)
                        Debug.Log("[SetPathFind] Node " + current.Connected[i] + " has few connections");
                    connectedVals[i] = int.MaxValue;
                }else if (tested.Contains(current.Connected[i]))
                {
                    if (debug)
                        Debug.Log("[SetPathFind] Node " + current.Connected[i] + "Alreaded tested");
                    //if we have already tested a discarded a point, max val
                    connectedVals[i] = int.MaxValue;
                }else if(result.Count > 1 && current.Connected[i] == result[result.Count - 1])
                {
                    if (debug)
                        Debug.Log("[SetPathFind] Node " + current.Connected[i] + " is previous point");
                    //If the point is the previous point, max val
                    connectedVals[i] = int.MaxValue;

                }else if(result.Count > 1 && result.Contains(current.Connected[i]))
                {
                    if (debug)
                        Debug.Log("[SetPathFind] Node " + current.Connected[i] + " is circle");
                    connectedVals[i] = int.MaxValue;

                }
                else 
                {

                    connectedVals[i] = NodeValue(current.Connected[i], end);
                    if (debug)
                        Debug.Log("[SetPathFind] Node " + current.Connected[i] + " value of " + connectedVals[i]);
                }
                if (minVal == -1)
                {
                    minVal = connectedVals[i];
                    minNode = i;
                    if (debug)
                    {
                        Debug.Log("[SetPathFind] Setting min val to " + minVal);
                    }
                    
                }
                else
                {
                    if(connectedVals[i] < minVal)
                    {
                        minVal = connectedVals[i];

                        minNode = i;
                        if (debug)
                        {
                            Debug.Log("[SetPathFind] Setting min val to " + minVal);
                    }
                    }
                }
            }

            //This means that no 
            if(minVal > 1000000000)
            {
                //We take a step back
                
                
                if(result.Count == 1)
                {
                    if (debug)
                    {
                        Debug.Log("[SetPathFind] No possible path");
                    }
                    //If we find no valid direction on the first node,
                    //then there is no path
                    path = result;
                    return false;
                }
                if (debug)
                {
                    Debug.Log("[SetPathFind] Step back ");
                }
                //Ensure we don't check this one again
                tested.Add(current);
                //Move back 1 space
                current = result[result.Count - 1];
                //Remove from result
                result.RemoveAt(result.Count - 1);

            }
            else
            {
                if (debug)
                {
                    Debug.Log("[SetPathFind] Next node: " + current.Connected[minNode]);
                }
                result.Add(current.Connected[minNode]);
                current = current.Connected[minNode];
            }

        }
        path = result;
        return false;

    }

    private static int NodeValue(SettlementPathNode node, SettlementPathNode end)
    {
        Vec2i disp = node.Position - end.Position;
        return disp.x * disp.x + disp.z * disp.z;
    }

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

    public bool IsMain;

    public Vec2i Position { get; private set; }
    public SettlementPathNode[] Connected;
    public int ConnectedCount { get; private set; }
    public SettlementPathNode(Vec2i nodePosition)
    {
        Position = nodePosition;
        Connected = new SettlementPathNode[4];
        ConnectedCount = 0;
    }

    public void AddConnection(int direction, SettlementPathNode node)
    {
        //If the direction was NOT null, and will be set to null, our connection count goes down
        if (Connected[direction] != null && node == null)
            ConnectedCount -= 1;
        else if (Connected[direction] == null && node != null)
            ConnectedCount += 1;
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

    public override string ToString()
    {
        if (Position == null)
            return "error";
        return Position.ToString();
    }

}