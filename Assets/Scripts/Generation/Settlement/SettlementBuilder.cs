using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
[System.Serializable]
public enum SettlementType
{
    CAPITAL, CITY, TOWN, VILLAGE
}
public class SettlementBuilder
{

    public static bool TEST = true;

    private GenerationRandom GenerationRandom;

    private GameGenerator GameGenerator;
    //Defines the minimum xz coordinate of this settlement, such that world positions are equal to BaseCoord+localPosition
    public Vec2i BaseCoord { get; private set; }
    //Defines the middle tile of the Settelment in local coordinates
    private Vec2i MidTile { get; }
    //The chunk coordinate at the centre of the settlement, defines its position in the world
    public Vec2i Centre { get; private set; }
    public Vec2i BaseChunk;

    //An array of all the chunks that this settelment contains/belongs to.
    public Vec2i[] SettlementChunks { get; private set; }

    //Array that holds all WorkObjects that will exist in this settlement
    public WorldObjectData[,] SettlementObjects { get; set; }
    //Array that hold all Tiles for this settlement
    public Tile[,] Tiles { get; set; }

    //Defines the size of the settlement from one edge to another, in units of tiles.
    public int TileSize { get; private set; }
    public SettlementType SettlementType { get; private set; }
    public List<Building> Buildings { get; private set; }
    //A list that defines the mid point of the settelment entrances, up to 4 (one on each face) can be defined.
    public Vec2i Entrance { get; private set; }
    public SettlementPathNode EntranceNode;
    public int EntranceNodeDirection;
    //A list of all points that are parts of a path. Contains duplicates which are removed when the final settlement is built
    public List<Vec2i> PathNodes { get; }
    public List<SettlementPathNode> TestNodes;
    //A list of the possible plots to build on.
    public List<Recti> BuildingPlots { get; }
    //Used to generate paths inside this settlement

    public Tile PathTile { get; private set; }

    public SettlementBuilder(GameGenerator gameGen, SettlementBase set)
    {
        GameGenerator = gameGen;
        if(gameGen != null)
            GenerationRandom = new GenerationRandom(gameGen.Seed);
        else
            GenerationRandom = new GenerationRandom(0);
        BaseChunk = set.BaseChunk;
        Centre = set.Centre;
        BaseCoord = set.BaseCoord;
        SettlementChunks = set.SettlementChunks;
        TileSize = set.TileSize;
        PathTile = Tile.STONE_PATH;
        MidTile = new Vec2i(TileSize / 2, TileSize / 2);
        Tiles = new Tile[TileSize, TileSize];
        SettlementObjects = new WorldObjectData[TileSize, TileSize];
        Buildings = new List<Building>();
        PathNodes = new List<Vec2i>();
        BuildingPlots = new List<Recti>();
        SettlementType = set.SettlementType;
        TestNodes = new List<SettlementPathNode>();
    }


    public void GenerateSettlement()
    {
        ChooseRandomEntrancePoints();
        List<BuildingPlan> mustAdd = new List<BuildingPlan>();
        
        BuildingPlan defaultRemaining = Building.HOUSE;
        switch (SettlementType)
        {
            case SettlementType.CAPITAL:

                AddMainBuilding(BuildingGenerator.GenerateCastle(48));

                mustAdd.Add(Building.MARKET);
                mustAdd.Add(Building.BARACKSCITY);
                mustAdd.Add(Building.TAVERN);
                mustAdd.Add(Building.BLACKSMITH);
                mustAdd.Add(Building.ALCHEMISTS);
                mustAdd.Add(Building.TAVERN);
                mustAdd.Add(Building.BLACKSMITH);
                mustAdd.Add(Building.TAVERN);
                mustAdd.Add(Building.GENERALMERCHANT);
                mustAdd.Add(Building.ARCHERYSTORE);
                mustAdd.Add(Building.SWORDSELLER);
                
                for(int i=0; i<40; i++)
                {
                    mustAdd.Add(Building.HOUSE);
                }

                defaultRemaining = Building.HOUSE;
                break;
            case SettlementType.CITY:
                AddMainBuilding(BuildingGenerator.GenerateCastle(32));
                mustAdd.Add(Building.BARACKSCITY);
                mustAdd.Add(Building.BLACKSMITH);
                mustAdd.Add(Building.GENERALMERCHANT);
                mustAdd.Add(Building.TAVERN);
                mustAdd.Add(Building.MARKET);

                for (int i = 0; i < 30; i++)
                {
                    mustAdd.Add(Building.HOUSE);
                }
                break;
            case SettlementType.TOWN:
                AddMainBuilding(BuildingGenerator.CreateBuilding(Building.HOLD));
                mustAdd.Add(Building.BARACKSTOWN);
                mustAdd.Add(Building.BLACKSMITH);
                mustAdd.Add(Building.GENERALMERCHANT);
                mustAdd.Add(Building.TAVERN);
                mustAdd.Add(Building.MARKET);
                for (int i = 0; i < 20; i++)
                {
                    mustAdd.Add(Building.HOUSE);
                }
                break;

            case SettlementType.VILLAGE:
                AddMainBuilding(BuildingGenerator.CreateBuilding(Building.VILLAGEHALL));
                mustAdd.Add(Building.BLACKSMITH);
                mustAdd.Add(Building.GENERALMERCHANT);
                mustAdd.Add(Building.TAVERN);
                mustAdd.Add(Building.MARKET);
                for (int i = 0; i < 15; i++)
                {
                    mustAdd.Add(Building.HOUSE);
                }
                break;
        }

        PlaceBuildings(mustAdd);

    }

    private void PlaceBuildings(List<BuildingPlan> buildings)
    {


        foreach (BuildingPlan bp in buildings)
        {
            Building b = BuildingGenerator.CreateBuilding(bp);
            Recti r = null;
            int i = 0;
            while (r == null && i < 5)
            {
                r = AddBuilding(b);
                i++;
            }
            if (r == null)
                continue;

            SurroundByPath(r.X, r.Y, r.Width, r.Height, 2);
            SettlementPathNode[] nodes = AddPlot(r);


        }

    }





    private SettlementPathNode[] AddPlot(Recti r)
    {
        SettlementPathNode[] nodes = new SettlementPathNode[4];
        if (r.X > 0 && r.Y > 0)
        {
            Vec2i n1 = new Vec2i(r.X - 1, r.Y - 1);
            nodes[0] = new SettlementPathNode(n1); //Bottom left
            PathNodes.Add(new Vec2i(r.X - 1, r.Y - 1));
        }
        if (r.X > 0 && r.X + r.Width + 1 < TileSize && r.Y > 0)
        {
            PathNodes.Add(new Vec2i(r.X + r.Width + 1, r.Y - 1));
            nodes[1] = new SettlementPathNode(new Vec2i(r.X + r.Width + 1, r.Y - 1)); //Bottom right
        }
        if (r.X > 0 && r.Y > 0 && r.Y + r.Height + 1 < TileSize)
        {
            PathNodes.Add(new Vec2i(r.X - 1, r.Y + r.Height + 1));
            nodes[2] = new SettlementPathNode(new Vec2i(r.X - 1, r.Y + r.Height + 1)); //Top Left

        }
        if (r.X > 0 && r.X + r.Width + 1 < TileSize && r.Y > 0 && r.Y + r.Height + 1 < TileSize)
        {
            PathNodes.Add(new Vec2i(r.X + r.Width + 1, r.Y + r.Height + 1));
            nodes[3] = new SettlementPathNode(new Vec2i(r.X + r.Width + 1, r.Y + r.Height + 1)); //Top Right
        }
        if (nodes[0] != null)
        {
            if (nodes[1] != null)
            {
                nodes[0].AddConnection(SettlementPathNode.EAST, nodes[1]);
                nodes[1].AddConnection(SettlementPathNode.WEST, nodes[0]);
            }
            if (nodes[2] != null)
            {
                nodes[0].AddConnection(SettlementPathNode.NORTH, nodes[2]);
                nodes[2].AddConnection(SettlementPathNode.SOUTH, nodes[0]);
            }
        }
        if (nodes[3] != null)
        {
            if (nodes[1] != null)
            {
                nodes[3].AddConnection(SettlementPathNode.SOUTH, nodes[1]);
                nodes[1].AddConnection(SettlementPathNode.NORTH, nodes[3]);
            }
            if (nodes[2] != null)
            {
                nodes[3].AddConnection(SettlementPathNode.WEST, nodes[2]);
                nodes[2].AddConnection(SettlementPathNode.EAST, nodes[3]);
            }
        }
        TestNodes.AddRange(nodes);
        BuildingPlots.Add(r);
        return nodes;

    }
    /// <summary>
    /// Surrounds the given region by a path of width 'pathSize'
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="pathSize"></param>
    public void SurroundByPath(int x, int z, int width, int height, int pathSize)
    {
        //iterate pathsize
        for (int i = 0; i < pathSize; i++)
        {
            //Iterate along x axis
            for (int x_ = x - pathSize; x_ < x + width + pathSize; x_++)
            {
                //Add path for each x point at minimum and maximum z values
                SetTile(x_, z - 1 - i, Tile.STONE_FLOOR);
                SetTile(x_, z + height + i, Tile.STONE_FLOOR);
            }
            for (int z_ = z; z_ <= z + height; z_++)
            {
                //Add path for each z point at minimum and maximum x values
                SetTile(x - 1 - i, z_, Tile.STONE_FLOOR);
                SetTile(x + width + i, z_, Tile.STONE_FLOOR);
            }
        }


    }


    public bool PointFree(int x, int z)
    {
        if (x < 0 || x >= TileSize || z < 0 || z >= TileSize)
        {
            return false;
        }
        return Tiles[x, z] == null && SettlementObjects[x, z] == null;
    }



    public Vec2i GetFreePoint()
    {
        Vec2i toOut = GenerationRandom.RandomVec2i(1, TileSize - 2);
        //Vec2i toOut = new Vec2i(MiscMaths.RandomRange(1, TileSize - 2), MiscMaths.RandomRange(1, TileSize - 2));
        while (true)
        {
            if (Tiles[toOut.x, toOut.z] == null && SettlementObjects[toOut.x, toOut.z] == null)
                return toOut;
            toOut = GenerationRandom.RandomVec2i(1, TileSize - 2);

        }


    }

    public void ChooseRandomEntrancePoints()
    {
        bool[] dirs = new bool[] { false, false, false, false };

        dirs[GenerationRandom.RandomInt(0, 3)] = true;

        int eMin = TileSize / 4;
        int eMax = TileSize - eMin;

        if (dirs[0])
        {
            Entrance = 
            Entrance = new Vec2i(0, GenerationRandom.RandomInt(eMin, eMax));
            EntranceNodeDirection = SettlementPathNode.EAST;
        }
        if (dirs[2])
        {
            Entrance = new Vec2i(TileSize - 1, GenerationRandom.RandomInt(eMin, eMax));
            EntranceNodeDirection = SettlementPathNode.WEST;

        }


        if (dirs[1])
        {
            Entrance = new Vec2i(GenerationRandom.RandomInt(eMin, eMax), TileSize - 1);
            EntranceNodeDirection = SettlementPathNode.SOUTH;

        }
        if (dirs[3])
        {
            Entrance = new Vec2i(GenerationRandom.RandomInt(eMin, eMax), 0);
            EntranceNodeDirection = SettlementPathNode.NORTH;


        }
        TestNodes.Add(new SettlementPathNode(Entrance));

        EntranceNode = new SettlementPathNode(Entrance);
        for (int x = -2; x <= 2; x++)
        {
            for (int z = -2; z <= 2; z++)
            {
                SetTile(Entrance.x + x, Entrance.z + z, Tile.TEST_YELLOW);
            }
        }

    }

    /// <summary>
    /// Adds the given main building to a point near the centre of the settlement, then surrounds it
    /// by a large path
    /// </summary>
    /// <param name="building"></param>
    public void AddMainBuilding(Building building)
    {
        //Choose the position and place building
        //Vec2i pos = MidTile + new Vec2i(MiscMaths.RandomRange(-20, 20), MiscMaths.RandomRange(-20, 20));

        Recti r = null;

        while(r==null)
            r = AddBuilding(building);
        //Add the plot and collect the path nodes
        SettlementPathNode[] nodes = AddPlot(r);

        //Find which node is nearest the settlement entrance.
        int nearestNodeToEntrace = 0;
        for (int i = 0; i < 4; i++)
        {
            if (nodes[i] == null)
                continue;
            if (Vec2i.QuickDistance(nodes[i].Position, Entrance) < Vec2i.QuickDistance(nodes[nearestNodeToEntrace].Position, Entrance))
                nearestNodeToEntrace = i;
        }

        ConnectEntranceNode(nodes[nearestNodeToEntrace], 5);
        ConnectPathNodes(EntranceNode, nodes[nearestNodeToEntrace], 5);
        //CreatePathFromNode(nodes[nearestNodeToEntrace], 4);
        SurroundByPath(r.X, r.Y, r.Width, r.Height, 5);
        //BuildingPlots.Add(r);
    }

    /// <summary>
    /// Connects the given node to the entrance node
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="width"></param>
    private void ConnectEntranceNode(SettlementPathNode startNode, int width)
    {
        SettlementPathNode entranceNode = EntranceNode;
        int entranceDirection = EntranceNodeDirection;
        Vec2i entranceDirectionStep = SettlementPathNode.GetDirection(entranceDirection);
        Vec2i diff = startNode.Position - entranceNode.Position;

        Vec2i midNodePosition = entranceNode.Position + new Vec2i(Mathf.Abs(diff.x) * entranceDirectionStep.x, Mathf.Abs(diff.z) * entranceDirectionStep.z);
        SettlementPathNode midNode = new SettlementPathNode(midNodePosition);
        entranceNode.AddConnection(entranceDirection, midNode);
        midNode.AddConnection(SettlementPathNode.OppositeDirection(entranceDirection), entranceNode);
        TestNodes.Add(midNode);


    }



    public void ConnectPathNodes(SettlementPathNode first, SettlementPathNode second, int width)
    {
        Vec2i diff = second.Position - first.Position; //Find the vector between the two nodes
        //First generate the path in the x direction
        Vec2i xDiff = new Vec2i(diff.x, 0);
        int xDiffDirection = SettlementPathNode.GetDirection(xDiff);
        SettlementPathNode midNode = CreatePathFromNode(first, width, chosenDirection: xDiffDirection, length: Mathf.Abs(diff.x));
        TestNodes.Add(midNode);
        //Add connections
        first.AddConnection(xDiffDirection, midNode);
        midNode.AddConnection(SettlementPathNode.OppositeDirection(xDiffDirection), first);

        Vec2i zDiff = new Vec2i(0, diff.z);
        ConnectNodes(midNode, second, width);

    }


    public void ConnectNodes(SettlementPathNode first, SettlementPathNode second, int width)
    {
        Vec2i diff = second.Position - first.Position; //Find the vector between the two nodes

        int length = diff.x + diff.z;
        int direction = SettlementPathNode.GetDirection(diff);
        first.AddConnection(direction, second);
        second.AddConnection(SettlementPathNode.OppositeDirection(direction), first);
        Vec2i step = SettlementPathNode.GetDirection(direction);
        Vec2i perpDirection = SettlementPathNode.GetPerpendicular(direction);
        int halfWidth = width / 2;

        for (int l = 0; l < length; l++)
        {
            for (int w = -halfWidth; w <= halfWidth; w++)
            {
                Vec2i pos = first.Position + step * l + perpDirection * w;
                SetTile(pos.x, pos.z, Tile.TEST_BLUE);
            }
        }
    }

    public void CreatePathBetweenNodes(SettlementPathNode firstNode, int secondNodeDirection, int width, int length = -1)
    {
        SettlementPathNode second = firstNode.Connected[secondNodeDirection];
        if (second == null)
        {
            Debug.Error("Paths are not connected");
            return;
        }

        Vec2i pathDiff = second.Position - firstNode.Position; //Find the vector that describes first->second
        int pathDiffLen = pathDiff.x + pathDiff.z; //Vector should have only 1 component, so total length can be written as sum
        int startPointOffset = pathDiffLen / 4;
        int startPoint = MiscMaths.RandomRange(startPointOffset, pathDiffLen - startPointOffset);

        //Generate node between two existing nodes
        SettlementPathNode interPathNode = new SettlementPathNode(firstNode.Position + SettlementPathNode.GetDirection(secondNodeDirection) * startPoint);
        int oppositeDir = SettlementPathNode.OppositeDirection(secondNodeDirection);
        TestNodes.Add(interPathNode);
        //Update connections
        firstNode.AddConnection(secondNodeDirection, interPathNode);
        interPathNode.AddConnection(oppositeDir, firstNode);
        second.AddConnection(oppositeDir, interPathNode);
        interPathNode.AddConnection(secondNodeDirection, second);
        CreatePathFromNode(interPathNode, width, length: length);

    }


    public SettlementPathNode CreatePathFromNode(SettlementPathNode node, int width, bool extraLength = false, int chosenDirection = -1, int length = -1)
    {
        //If no direction is given, choose a null one
        if (chosenDirection == -1)
        {
            List<int> nullDirection = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (node.Connected[i] == null)
                    nullDirection.Add(i);
            }
            Debug.Log(nullDirection.Count);
            //Choose a valid direction and find the vector step
            chosenDirection = GenerationRandom.RandomFromList(nullDirection);
        }

        Vec2i step = SettlementPathNode.GetDirection(chosenDirection);
        //If no length is given or given length is invalid, choose a path length
        if (length == -1 || !InBounds(node.Position + step * length))
        {
            int attemptLength = length == -1 ? GenerationRandom.RandomInt(40, TileSize) : length;
            while (!InBounds(node.Position + step * attemptLength))
                attemptLength -= 1;

            length = attemptLength;
        }
        int halfWidth = width / 2;
        Vec2i perpDirection = SettlementPathNode.GetPerpendicular(chosenDirection);
        if (extraLength)
            length += halfWidth;
        for (int l = 0; l < length; l++)
        {
            for (int w = -halfWidth; w <= halfWidth; w++)
            {
                Vec2i pos = node.Position + step * l + perpDirection * w;
                SetTile(pos.x, pos.z, Tile.TEST_BLUE);
            }
        }
        SettlementPathNode endNode = new SettlementPathNode(node.Position + step * length);
        node.AddConnection(chosenDirection, endNode);
        endNode.AddConnection(SettlementPathNode.OppositeDirection(chosenDirection), node);
        return endNode;
    }


    public void SetTile(int x, int z, Tile tile)
    {
        if (x < 0 || x >= TileSize || z < 0 || z >= TileSize)
            return;
        Tiles[x, z] = tile;
    }
    /// <summary>
    /// Sets all tiles on the ground to this tile
    /// </summary>
    /// <param name="tile"></param>
    public void SetBaseTile(Tile tile)
    {
        for (int x = 0; x < TileSize; x++)
        {
            for (int z = 0; z < TileSize; z++)
            {
                if (Tiles[x, z] == null)
                    Tiles[x, z] = tile;
            }
        }
    }

    public bool InBounds(Vec2i v)
    {
        return v.x >= 0 && v.x < TileSize && v.z >= 0 && v.z < TileSize;
    }
    public bool InBounds(int x, int z)
    {
        return x >= 0 && x < TileSize && z >= 0 && z < TileSize;
    }
    public Vec2i ChoosePlot(int width, int height, int attempts = 20)
    {

        for (int i = 0; i < attempts; i++)
        {
            Vec2i pos = GenerationRandom.RandomVec2i(0, TileSize - 1 - width);
            if (IsAreaFree(pos.x, pos.z, width, height))
            {
                return pos;
            }
        }
        return null;
    }






    public Recti AddBuilding(Building b, Vec2i pos = null, bool force = false)
    {
        if (pos != null)
        {

            if (force == false && !IsAreaFree(pos.x, pos.z, b.Width, b.Height))
                return null;
        }
        else
        {
            pos = ChoosePlot(b.Width, b.Height);
        }
        //We choose a random allowed position.
        //Vec2i pos = ChoosePlot(b.Width, b.Height);

        //If no possible position is found, we return null
        if (pos == null)
            return null;

        if (force)
        {
            if (pos.x + b.Width >= TileSize || pos.z + b.Height >= TileSize)
                return null;
        }

        for (int x = 0; x < b.Width; x++)
        {
            for (int z = 0; z < b.Height; z++)
            {
                Tiles[x + pos.x, z + pos.z] = b.BuildingTiles[x, z];
                SettlementObjects[x + pos.x, z + pos.z] = b.BuildingObjects[x, z];
                if(SettlementObjects[x + pos.x, z + pos.z]!=null)
                    SettlementObjects[x + pos.x, z + pos.z].SetPosition(new Vec2i(x + pos.x, z + pos.z));
            }
        }
        b.SetSettlementCoord(pos);
        Buildings.Add(b);
        PathNodes.Add(b.Entrance);
        return new Recti(pos.x, pos.z, b.Width, b.Height);
    }

    public bool IsAreaFree(int x, int z, int width, int height)
    {
        if(GameGenerator != null)
        {
            Vec2i baseChunk = BaseChunk + new Vec2i(Mathf.FloorToInt((float)x / World.ChunkSize), Mathf.FloorToInt((float)z / World.ChunkSize));
            Vec2i cBase = World.GetChunkPosition(this.BaseCoord + new Vec2i(x, z)) - new Vec2i(2,2);
            int chunkWidth = Mathf.FloorToInt((float)width / World.ChunkSize)+1;
            int chunkHeight = Mathf.FloorToInt((float)height / World.ChunkSize)+1;

            for (int cx = 0; cx < chunkWidth; cx++)
            {
                for (int cz = 0; cz < chunkHeight; cz++)
                {
                    int cbx = baseChunk.x + cx;
                    int cbz = baseChunk.z + cz;
                    if (cbx < 0 || cbx > World.WorldSize - 1 || cbz < 0 || cbz > World.WorldSize - 1)
                        return false;
                    ChunkBase cb = GameGenerator.TerrainGenerator.ChunkBases[cbx, cbz];
                    
                    if(cb.RiverNode != null)
                    {
                        cb.RiverNode.AddBridge(new RiverBridge());
                        return false;
                    }
                    if (cb.RiverNode != null || cb.Lake != null || !cb.IsLand)
                    {
                        
                        return false;
                    }
                }
            }
        }

        for (int x_ = x; x_ < x + width; x_++)
        {
            for (int z_ = z; z_ < z + height; z_++)
            {
                if (z_ >= TileSize || x_ >= TileSize)
                {
                    return false;
                }
                if (Tiles[x_, z_] != null)
                    return false;
            }
        }

        return true;
    }


}