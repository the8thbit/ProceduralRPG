using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
/// <summary>
/// A river is defined by a set of chunks which it passes through
/// </summary>
public class River
{
    private List<Vec2i> RiverChunks;
    private List<float> RiverWidths;
    public River()
    {
        RiverChunks = new List<Vec2i>();
        RiverWidths = new List<float>();
    }


    /// <summary>
    /// Defines the first chunk for the river
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="width"></param>
    public void SetFirstChunk(Vec2i chunk, float width)
    {
        //Clear lists to prepair for first chunk
        RiverChunks.Clear();
        RiverWidths.Clear();

        RiverChunks.Add(chunk);
        RiverWidths.Add(width);
    }

    /// <summary>
    /// Checks if the supplied chunk can be added to this river.
    /// If possible, add the chunk to this river and return true
    /// otherwise, return false
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public bool AddChunk(Vec2i chunk, float width)
    {
        //Get the last chunk
        Vec2i last = RiverChunks[RiverChunks.Count - 1];
        if (last == chunk)
            return false;
        else if (RiverChunks.Count > 2 && RiverChunks[RiverChunks.Count - 2] == chunk)
            return false;
        //Check if they're neighborings
        if (Neighboring(last, chunk, true))
        {
            RiverChunks.Add(chunk);
            RiverWidths.Add(width);
            return true;
        }
        return false;

    }


    /// <summary>
    /// Returns true if the two supplied positions are neighbors.
    /// If diag is true, then neighbors include diagonal elements.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool Neighboring(Vec2i a, Vec2i b, bool diag=false)
    {
        //Get the distance in each axis between positions
        int dx = (a.x - b.x) * (a.x - b.x);
        int dz = (a.z - b.z) * (a.z - b.z);
        //If we are checking only diret neigbors..
        if (!diag)
        {
            return dx + dz == 1;
        }
        return dx <= 1 && dz <= 1;

    }

    /// <summary>
    /// Converts the set of chunks in this River to a dictionary of river nodes.
    /// The key represents the chunk this node belongs on.
    /// The Node contains details about the width of the river at the start and end node,
    /// as well as the path the river takes through the chunk.
    /// </summary>
    /// <returns></returns>
    public Dictionary<Vec2i, RiverNode> GenerateRiverNodes()
    {
        //Create dict for nodes, initiate with correct size 
        Dictionary<Vec2i, RiverNode> nodes = new Dictionary<Vec2i, RiverNode>(RiverChunks.Count + 1);

        //Iterate all chunks
        for(int i=0; i<RiverChunks.Count; i++)
        {
            Vec2i node = RiverChunks[i];
            //If this is the final chunk of a river, then it has no exit.
            //This means the first chunk of a river should be in ocean/lake
            Vec2i exit = (i == 0) ? null : RiverChunks[i - 1];
            float exitWidth = (i == 0) ? RiverWidths[0] : RiverWidths[i - 1];


            //Get the entrance to this node, if this is the final node
            Vec2i entr = (i == RiverChunks.Count - 1) ? null : RiverChunks[i + 1];
            float entrWidth = (i == RiverChunks.Count - 1) ? RiverWidths[RiverChunks.Count - 1] : RiverWidths[i + 1];

            if(nodes.ContainsKey(node) == false)
                nodes.Add(node, new RiverNode(node, entr, exit, entrWidth, exitWidth));

        }

        return nodes;
    }

}
/// <summary>
/// Object contains data passed to ChunkBase. This details which direction in and out of the 
/// chunk the river goes, as well as the width of the river at this point.
/// From this, the river is generated in 
/// </summary>
public class RiverNode : ChunkGenerationFeature
{
    public RiverBridge Bridge { get; private set; }
    public Vec2i NodePosition { get; }
    public Vec2i RiverEntrance { get; }
    public Vec2i RiverExit { get; }
    public float EntranceWidth { get; }
    public float ExitWidth { get; }

    //The delta the entrance and exit nodes are relative to this chunk
    public Vec2i RiverEntranceDelta { get; }
    public Vec2i RiverExitDelta { get; }

    private Vec2i RivDir;
    /// <summary>
    /// Constructor for river node, contains all needed information
    /// </summary>
    /// <param name="node"></param>
    /// <param name="entr"></param>
    /// <param name="exit"></param>
    /// <param name="entrWidth"></param>
    /// <param name="exitWidth"></param>
    public RiverNode(Vec2i node, Vec2i entr, Vec2i exit, float entrWidth, float exitWidth)
    {
        NodePosition = node;
        RiverEntrance = entr;
        RiverExit = exit;

        EntranceWidth = entrWidth;
        ExitWidth = exitWidth;

        if(entr != null)
            RiverEntranceDelta = entr - node;
        if(exit != null)
            RiverExitDelta = exit - node;

        //If the entrance is null, we are at the start of the river. The direction is its exit
        if(entr == null)
        {
            RivDir = new Vec2i(Mathf.Abs(RiverExitDelta.x), Mathf.Abs(RiverExitDelta.z));
        }else if(exit == null)
        {
            RivDir = new Vec2i(Mathf.Abs(RiverEntranceDelta.x), Mathf.Abs(RiverEntranceDelta.z));
        }
        else
        {
            int dx = 0;
            int dz = 0;
            if (entr.x == exit.x)
                dx = 0;
            else if (exit.x > entr.x)
                dx = 1;
            else
                dx = -1;

            if (entr.z == exit.z)
                dz = 0;
            else if (exit.z > entr.z)
                dz = 1;
            else
                dz = -1;
            Vec2i sum = RiverExit - RiverEntrance;
            RivDir = new Vec2i(dx, dz);
        }



    }

    public void AddBridge(RiverBridge b)
    {
        Bridge = b;
    }

    public Vec2i RiverNodeDirection()
    {
        return RivDir;
    }

}