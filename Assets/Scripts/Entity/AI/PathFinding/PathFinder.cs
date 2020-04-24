using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
public class PathFinder
{
    public static readonly int RegionTileSize = World.RegionSize * World.ChunkSize;
    public static readonly Vec2i[] DIRS = new[] {
    new Vec2i(1, 0), // to right of tile
    new Vec2i(0, -1), // below tile
    new Vec2i(-1, 0), // to left of tile
    new Vec2i(0, 1), // above tile
    new Vec2i(1, 1), // diagonal top right
    new Vec2i(-1, 1), // diagonal top left
    new Vec2i(1, -1), // diagonal bottom right
    new Vec2i(-1, -1) // diagonal bottom left
  };
    public static int PathFinderSize = World.ChunkSize * (2 * WorldManager.LoadChunkRadius + 1);

    //private float[,] TileValues;
    private Dictionary<Vec2i, float[,]> RegionTileValues;

    private float[,] CurrentSubworldTiles;
    private Subworld Subworld;
    private Vec2i SubworldSize;

    private Vec2i LastMid;
    private Vec2i OffSet;
    public World World { get; private set; }
    public PathFinder(World world)
    {
        //TileValues = new float[PathFinderSize, PathFinderSize];
        World = world;
        RegionTileValues = new Dictionary<Vec2i, float[,]>();
    }

    /// <summary>
    /// Generates a map of moveable tiles based on the given region
    /// </summary>
    /// <param name="chunkRegion"></param>
    public void LoadRegion(ChunkRegion chunkRegion)
    {
        Debug.Log("[PathFinding] Chunk Region " + chunkRegion.X + "," + chunkRegion.Z + " loading");
        //Get region position & generate empty array
        Vec2i rPos = new Vec2i(chunkRegion.X, chunkRegion.Z);
        float[,] tileData = new float[RegionTileSize, RegionTileSize];
        //Iterate whole region and fill array as required
        //TODO - see if there is a quicker way to do this...
        for(int cx=0; cx<World.RegionSize; cx++)
        {
            for(int cz=0; cz<World.RegionSize; cz++)
            {
                for(int x=0; x<World.ChunkSize; x++)
                {
                    for(int z=0; z<World.ChunkSize; z++)
                    {
                        ChunkData c = chunkRegion.Chunks[cx, cz];
                        if (c == null)
                        {
                            Debug.Error("Chunk Region " + chunkRegion.ToString() + " has null chunk with local: " + cx + "," + cz);
                            continue;
                        }
                        float val = tileData[cx * World.ChunkSize + x, cz * World.ChunkSize + z] = c.GetTile(x, z).SpeedMultiplier;
                        if (c.Objects != null && c.GetObject(x,z) != null)
                        {
                            WorldObjectData wod = c.GetObject(x, z);
                            if(wod.IsCollision)
                                val = Mathf.Infinity;
                        }

                        tileData[cx * World.ChunkSize + x, cz * World.ChunkSize + z] = val;
                    }
                }
            }
        }
        RegionTileValues.Add(rPos, tileData);
    }

    public void LoadSubworld(Subworld subworld)
    {
        Subworld = subworld;
        SubworldSize = subworld.ChunkSize * World.ChunkSize;
        CurrentSubworldTiles = new float[SubworldSize.x, SubworldSize.z];
        for (int x=0; x<subworld.ChunkSize.x; x++)
        {
            for (int z = 0; z < subworld.ChunkSize.z; z++)
            {
                for(int x_=0; x_<World.ChunkSize; x_++)
                {
                    for (int z_ = 0; z_ < World.ChunkSize; z_++)
                    {
                        ChunkData cd = subworld.SubworldChunks[x, z];
                        float val = cd.GetTile(x_, z_).SpeedMultiplier;
                        if(cd.Objects != null && cd.GetObject(x_,z_)!=null)
                        {
                            val = Mathf.Infinity;
                        }
                        CurrentSubworldTiles[x * World.ChunkSize + x_, z * World.ChunkSize + z_] = val;
                    }
                }
            }
        }
    }
    public void LeaveSubworld()
    {
        CurrentSubworldTiles = null;
        Subworld = null;
        SubworldSize = null;
    }

    public void UnloadRegion(Vec2i v)
    {
        if (RegionTileValues.ContainsKey(v))
        {
            RegionTileValues.Remove(v);
        }
    }

  
    public static int WorldDimensionToRegionDimension(int r)
    {
        return Mathf.FloorToInt(r / ((float)RegionTileSize));
    }
    public float NodeValue(Vec2i v)
    {
        if (Subworld != null)
            return SubworldNodeValue(v);
        Vec2i rPos = new Vec2i(WorldDimensionToRegionDimension(v.x), WorldDimensionToRegionDimension(v.z));
        int locX = v.x % RegionTileSize;
        int locZ = v.z % RegionTileSize;
 
        return RegionTileValues[rPos][locX, locZ];

    }

    private float SubworldNodeValue(Vec2i v)
    {
        if (v.x < 0 || v.x >= SubworldSize.x || v.z < 0 || v.z >= SubworldSize.z)
            return float.MaxValue;
        return CurrentSubworldTiles[v.x, v.z];
    }



    public bool InBounds(Vec2i v)
    {
        Vec2i rPos = new Vec2i(WorldDimensionToRegionDimension(v.x), WorldDimensionToRegionDimension(v.z));

        return RegionTileValues.ContainsKey(rPos);
        //return (x <= v.x) && (v.x < width) && (y <= id.y) && (id.y < height);
    }

    // Everything that isn't a Wall is Passable
    public bool Passable(Vec2i id)
    {
        return (int)NodeValue(id) < System.Int32.MaxValue;
    }

    // If the heuristic = 2f, the movement is diagonal
    public float Cost(Vec2i a, Vec2i b)
    {
        if (Heuristic(a, b) == 2f)
        {
            return (float)NodeValue(b) * Mathf.Sqrt(2f);
        }
        return (float)NodeValue(b);
    }

    // Check the tiles that are next to, above, below, or diagonal to
    // this tile, and return them if they're within the game bounds and passable
    public IEnumerable<Vec2i> Neighbors(Vec2i id)
    {
        foreach (var dir in DIRS)
        {
            Vec2i next = new Vec2i(id.x + dir.x, id.z + dir.z);
            if (InBounds(next) && Passable(next))
            {
                yield return next;
            }
        }
    }

    public Dictionary<Vec2i, Vec2i> cameFrom = new Dictionary<Vec2i, Vec2i>(2000);
    public Dictionary<Vec2i, float> costSoFar = new Dictionary<Vec2i, float>(2000);

    private Vec2i start;
    private Vec2i goal;

    static public float Heuristic(Vec2i a, Vec2i b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z);
    }


    public class PriorityQueue<T>
    {
        // From Red Blob: I'm using an unsorted array for this example, but ideally this
        // would be a binary heap. Find a binary heap class:
        // * https://bitbucket.org/BlueRaja/high-speed-priority-queue-for-c/wiki/Home
        // * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
        // * http://xfleury.github.io/graphsearch.html
        // * http://stackoverflow.com/questions/102398/priority-queue-in-net

        private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>(2000);

        public int Count
        {
            get { return elements.Count; }
        }

        public void Enqueue(T item, float priority)
        {
            elements.Add(new KeyValuePair<T, float>(item, priority));
        }

        // Returns the Location that has the lowest priority
        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Value < elements[bestIndex].Value)
                {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].Key;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }

    public List<Vec2i> GeneratePath(Vec2i start, Vec2i end)
    {
        //TODO - FIX THIS???
        cameFrom.Clear();
        costSoFar.Clear();
        var frontier = new PriorityQueue<Vec2i>();
        goal = end;
        this.start = start;
        frontier.Enqueue(start, 0f);

        cameFrom.Add(start, start); // is set to start, None in example
        costSoFar.Add(start, 0f);

        while (frontier.Count > 0f)
        {

            // Get the Location from the frontier that has the lowest
            // priority, then remove that Location from the frontier
            Vec2i current = frontier.Dequeue();

            // If we're at the goal Location, stop looking.
            if (current.Equals(goal))
            {
                break;
            }
            // Neighbors will return a List of valid tile Locations
            // that are next to, diagonal to, above or below current
            foreach (var neighbor in Neighbors(current))
            {

                // If neighbor is diagonal to current, graph.Cost(current,neighbor)
                // will return Sqrt(2). Otherwise it will return only the cost of
                // the neighbor, which depends on its type, as set in the TileType enum.
                // So if this is a normal floor tile (1) and it's neighbor is an
                // adjacent (not diagonal) floor tile (1), newCost will be 2,
                // or if the neighbor is diagonal, 1+Sqrt(2). And that will be the
                // value assigned to costSoFar[neighbor] below.
                float newCost = costSoFar[current] + Cost(current, neighbor);

                // If there's no cost assigned to the neighbor yet, or if the new
                // cost is lower than the assigned one, add newCost for this neighbor
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {

                    // If we're replacing the previous cost, remove it
                    if (costSoFar.ContainsKey(neighbor))
                    {
                        costSoFar.Remove(neighbor);
                        cameFrom.Remove(neighbor);
                    }

                    costSoFar.Add(neighbor, newCost);
                    cameFrom.Add(neighbor, current);
                    float priority = newCost + Heuristic(neighbor, goal);
                    frontier.Enqueue(neighbor, priority);
                }
            }
        }
        return FindPath();
    }


    public List<Vec2i> FindPath()
    {

        List<Vec2i> path = new List<Vec2i>(1000);
        Vec2i current = goal;

        while (!current.Equals(start))
        {
            if (!cameFrom.ContainsKey(current))
            {
                return new List<Vec2i>(100);
            }
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }
   
}