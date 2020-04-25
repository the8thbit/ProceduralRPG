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
    public static int NODE_RES = 16;
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

    public SettlementPathNode[,] TestNodes2;

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
        TestNodes2 = new SettlementPathNode[TileSize / NODE_RES, TileSize / NODE_RES];
    }


    public void GenerateSettlement()
    {
        //ChooseRandomEntrancePoints();
        AddInitPaths();
        List<BuildingPlan> mustAdd = new List<BuildingPlan>();
        
        BuildingPlan defaultRemaining = Building.HOUSE;
        switch (SettlementType)
        {
            case SettlementType.CAPITAL:

                AddMainBuilding(BuildingGenerator.GenerateCastle(48));
                mustAdd.Add(Building.BLACKSMITH);
                mustAdd.Add(Building.BLACKSMITH);
                mustAdd.Add(Building.BLACKSMITH);
                mustAdd.Add(Building.BLACKSMITH);
                mustAdd.Add(Building.BLACKSMITH);
                mustAdd.Add(Building.BLACKSMITH);
                mustAdd.Add(Building.BLACKSMITH);
                mustAdd.Add(Building.BLACKSMITH);

                /*
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
                */
                for (int i=0; i<40; i++)
                {
                    mustAdd.Add(Building.HOUSE);
                }

                defaultRemaining = Building.HOUSE;
                break;
            case SettlementType.CITY:
                AddMainBuilding(BuildingGenerator.GenerateCastle(32));
                //mustAdd.Add(Building.BARACKSCITY);
                mustAdd.Add(Building.BLACKSMITH);
                /*
                mustAdd.Add(Building.GENERALMERCHANT);
                mustAdd.Add(Building.TAVERN);
                mustAdd.Add(Building.MARKET);
                */
                for (int i = 0; i < 30; i++)
                {
                    mustAdd.Add(Building.HOUSE);
                }
                break;
            case SettlementType.TOWN:
                AddMainBuilding(BuildingGenerator.CreateBuilding(Building.HOLD));
                //mustAdd.Add(Building.BARACKSTOWN);

                mustAdd.Add(Building.BLACKSMITH);
                /*mustAdd.Add(Building.GENERALMERCHANT);
                mustAdd.Add(Building.TAVERN);
                mustAdd.Add(Building.MARKET);*/
                for (int i = 0; i < 20; i++)
                {
                    mustAdd.Add(Building.HOUSE);
                }
                break;

            case SettlementType.VILLAGE:
                AddMainBuilding(BuildingGenerator.CreateBuilding(Building.VILLAGEHALL));
                mustAdd.Add(Building.BLACKSMITH);
                /*
                mustAdd.Add(Building.GENERALMERCHANT);
                mustAdd.Add(Building.TAVERN);
                mustAdd.Add(Building.MARKET);*/
                for (int i = 0; i < 15; i++)
                {
                    mustAdd.Add(Building.HOUSE);
                }
                break;
        }

        PlaceBuildings(mustAdd);
        CreatePathNodes();
    }
    public SettlementPathNode ENTR_NODE;
    private void AddInitPaths()
    {

        int nodeSize = TileSize / NODE_RES;

        int xStart = GenerationRandom.RandomInt(0+3, nodeSize - 3);
        int zLen = nodeSize - GenerationRandom.RandomInt(0, 3); //Ends 0-3 chunks from settlement end

        int zStartEast = GenerationRandom.RandomInt(2, nodeSize - 3);
        int xLenEast = GenerationRandom.RandomInt(nodeSize - xStart - 2, nodeSize - xStart);


        int zStartWest = GenerationRandom.RandomInt(2, nodeSize - 3);
        int xLenWest = GenerationRandom.RandomInt(xStart-2, xStart);


        //Add the path nodes along the z direction (start at south -> north)
        for (int z=0; z<zLen; z++)
        {
            TestNodes2[xStart, z] = new SettlementPathNode(new Vec2i(xStart * NODE_RES, z * NODE_RES));
            TestNodes2[xStart, z].IsMain = true;
            SetTile(xStart * NODE_RES, z * NODE_RES, Tile.TEST_BLUE);
        }
        ENTR_NODE = TestNodes2[xStart, 0];




        for (int x=0; x<xLenEast; x++)
        {
            TestNodes2[xStart + x, zStartEast] = new SettlementPathNode(new Vec2i((xStart + x) * NODE_RES, zStartEast * NODE_RES));
            TestNodes2[xStart + x, zStartEast].IsMain = true;
        }


        for(int x=0; x<xLenWest; x++)
        {
            TestNodes2[xStart - x, zStartWest] = new SettlementPathNode(new Vec2i((xStart - x) * NODE_RES, zStartWest * NODE_RES));
            TestNodes2[xStart - x, zStartWest].IsMain = true;
        }


        //Iterate all points, and connect everything
        //Note, this can be made faster. We can half iterations with some TODO
        for(int x=0; x<nodeSize; x++)
        {
            for (int z = 0; z < nodeSize; z++)
            {
                if (TestNodes2[x, z] == null)
                    continue;
                if (x > 0 && TestNodes2[x - 1, z] != null)
                {
                    TestNodes2[x, z].AddConnection(SettlementPathNode.WEST, TestNodes2[x - 1, z]);
                    ConnectNodes(TestNodes2[x, z], TestNodes2[x - 1, z], 5);
                }
                if (z > 0 && TestNodes2[x, z - 1] != null)
                {
                    TestNodes2[x, z].AddConnection(SettlementPathNode.SOUTH, TestNodes2[x , z - 1]);
                    ConnectNodes(TestNodes2[x, z], TestNodes2[x , z - 1], 5);

                }
                if (x<nodeSize-1 && TestNodes2[x+1, z] != null)
                {
                    TestNodes2[x, z].AddConnection(SettlementPathNode.EAST, TestNodes2[x + 1, z]);
                    ConnectNodes(TestNodes2[x, z], TestNodes2[x + 1, z], 5);

                }
                if (z < nodeSize-1 && TestNodes2[x, z + 1] != null)
                {
                    TestNodes2[x, z].AddConnection(SettlementPathNode.NORTH, TestNodes2[x, z + 1]);
                    ConnectNodes(TestNodes2[x, z], TestNodes2[x, z + 1], 5);

                }
            }
        }
        /*
        for(int z=0; z<zLen-1; z++)
        {
            ConnectNodes(TestNodes2[xStart, z], TestNodes2[xStart, z + 1], 5);
            TestNodes2[xStart, z].AddConnection(SettlementPathNode.NORTH, TestNodes2[xStart, z + 1]);
            TestNodes2[xStart, z+1].AddConnection(SettlementPathNode.SOUTH, TestNodes2[xStart, z]);

        }*/
    }
    private void CreatePathNodes()
    {
        int tSize = TileSize/ NODE_RES;
        int checkDist = 4;
        //Create a node at every node position not inside a building
        for(int x=0; x<tSize; x++)
        {
            for (int z = 0; z < tSize; z++)
            {
                if (TestNodes2[x, z] != null)
                    continue;

                //Define the position of this node in the settlement
                
                Vec2i pos = new Vec2i(x * NODE_RES, z * NODE_RES);
                if(IsTileFree(pos.x, pos.z))
                    TestNodes2[x, z] = new SettlementPathNode(new Vec2i(x * NODE_RES, z * NODE_RES));
                
            }
        }
      
        //Connect all nodes
        for (int x = 0; x < tSize; x++)
        {
            for (int z = 0; z < tSize; z++)
            {
                if (TestNodes2[x, z] == null)
                    continue;
                if(x != 0)
                {
                    SettlementPathNode minX=null;
                    //Iterate x values below this value until non-null value found
                    for(int i=1; i<Mathf.Min(x, checkDist); i++)
                    {
                        minX = TestNodes2[x - i, z];
                        if (minX == null)
                            continue;

                        if (IsAreaFree(minX.Position.x, minX.Position.z - 1, i * World.ChunkSize, 3, Tile.TEST_BLUE))
                            break;
                        else
                            minX = null;
                    }
                    if(minX!=null)
                        TestNodes2[x, z].AddConnection(SettlementPathNode.WEST, minX);

                }if(z != 0)
                {
                    SettlementPathNode minZ = null;
                    //Iterate z values below this value until non-null value found
                    for (int i = 1; i < Mathf.Min(z, checkDist); i++)
                    {
                        minZ = TestNodes2[x, z - i];
                        if (minZ == null)
                            continue;

                        if (IsAreaFree(minZ.Position.x, minZ.Position.z - 1, 3, i * World.ChunkSize, Tile.TEST_BLUE))
                            break;
                        else
                            minZ = null;
                    }
                    if (minZ != null)
                        TestNodes2[x, z].AddConnection(SettlementPathNode.SOUTH, minZ);


                }
                if(x != tSize - 1)
                {

                    SettlementPathNode maxX = null;
                    //Iterate x values below this value until non-null value found
                    for (int i = 1; i < Mathf.Min(tSize - x, checkDist); i++)
                    {
                        maxX = TestNodes2[x + i, z];
                        if (maxX == null)
                            continue;

                        if (IsAreaFree(TestNodes2[x, z].Position.x, TestNodes2[x, z].Position.z - 1, i * World.ChunkSize, 3, Tile.TEST_BLUE))
                            break;
                        else
                            maxX = null;
                    }
                    if (maxX != null)
                        TestNodes2[x, z].AddConnection(SettlementPathNode.EAST, maxX);

                    //TestNodes2[x, z].AddConnection(SettlementPathNode.EAST, TestNodes2[x + 1, z]);

                }
                if (z != tSize - 1)
                {
                    SettlementPathNode maxZ = null;
                    //Iterate x values below this value until non-null value found
                    for (int i = 1; i < Mathf.Min(tSize - z, checkDist); i++)
                    {
                        maxZ = TestNodes2[x, z+i];
                        if (maxZ == null)
                            continue;

                        if (IsAreaFree(TestNodes2[x, z].Position.x, TestNodes2[x, z].Position.z - 1,3,  i * World.ChunkSize, Tile.TEST_BLUE))
                            break;
                        else
                            maxZ = null;
                    }
                    if (maxZ != null)
                        TestNodes2[x, z].AddConnection(SettlementPathNode.NORTH, maxZ);

                    // TestNodes2[x, z].AddConnection(SettlementPathNode.NORTH, TestNodes2[x, z+1]);

                }

            }
            
        }
        SettlementPathNode[] nodes = new SettlementPathNode[4];
        //Remove circular path nodes
        for(int x=0; x<tSize-1; x++)
        {
            for (int z = 0; z < tSize - 1; z++)
            {
                nodes[0] = TestNodes2[x, z];
                nodes[1] = TestNodes2[x+1, z];
                nodes[2] = TestNodes2[x+1, z+1];
                nodes[3] = TestNodes2[x, z+1];
                //If all nodes are non null
                if (nodes[0] != null && nodes[1] != null && nodes[2] != null && nodes[3] != null)
                {
                    int destroy = GenerationRandom.RandomInt(0, 4);
                    while (nodes[destroy].IsMain)
                        destroy = GenerationRandom.RandomInt(0, 4);

                    int minCount = nodes[destroy].ConnectedCount;
                    for (int i = 1; i < 4; i++)
                    {
                        if (nodes[i].ConnectedCount < minCount && !nodes[i].IsMain)
                            destroy = i;
                    }

                    if (destroy == 0)
                    {
                        DestroyNode(x, z);
                        SettlementObjects[x * NODE_RES, z * NODE_RES] = new Tree(new Vec2i(x * NODE_RES, z * NODE_RES));
                    }

                    if (destroy == 1)
                    {
                        DestroyNode(x + 1, z);
                        SettlementObjects[(x+1) * NODE_RES, z * NODE_RES] = new Tree(new Vec2i((x+1) * NODE_RES, z * NODE_RES));

                    }
                    if (destroy == 2) { 
                        DestroyNode(x + 1, z + 1);
                        SettlementObjects[(x+1) * NODE_RES, (z+1) * NODE_RES] = new Tree(new Vec2i((x+1) * NODE_RES, (z+1) * NODE_RES));

                    }
                    if (destroy == 3){
                        DestroyNode(x, z + 1);
                        SettlementObjects[x * NODE_RES, (z + 1) * NODE_RES] = new Tree(new Vec2i(x * NODE_RES, (z + 1) * NODE_RES));

                    }
                }
            }
        }
        List<SettlementPathNode> island = new List<SettlementPathNode>();
        List<SettlementPathNode> tested = new List<SettlementPathNode>();
        //Remove islands
        for(int x=0; x<tSize; x++)
        {
            for (int z = 0; z < tSize; z++)
            {
                if (TestNodes2[x, z] == null)
                    continue;
                if (TestNodes2[x, z].ConnectedCount == 0)
                {
                    DestroyNode(x, z);
                    continue;
                }
                if (tested.Contains(TestNodes2[x, z]))
                    continue;
                if(TestNodes2[x,z].ConnectedCount == 1)
                {
                    
                    //If no path is found
                    if(!SettlementPathFinder.SettlementPath(TestNodes2[x, z], ENTR_NODE, out island))
                    {
                        Debug.Log("[SettlementBuilder] Island found");
                        foreach(SettlementPathNode node in island)
                        {
                            DestroyNode(node.Position.x / NODE_RES, node.Position.z / NODE_RES);
                        }
                        island.Clear();
                    }
                    else
                    {
                        tested.AddRange(island);
                        island.Clear();
                    }
                }
                   
            }
        }
        foreach(SettlementPathNode pn in TestNodes2)
        {
            if (pn == null)
                continue;
            
            for(int i=0; i<4; i++)
            {
                if(pn.Connected[i] != null)
                {
                    if (!(pn.IsMain && pn.Connected[i].IsMain))
                         ConnectNodes(pn, pn.Connected[i], 3);
                }
                   
            }
        }
    }

    private void DestroyNode(int x, int z)
    {
        SettlementPathNode node = TestNodes2[x, z];
        if (node == null)
            return;
        for(int i=0; i<4; i++)
        {
            SettlementPathNode coni = node.Connected[i];
            if(coni != null)
            {
                coni.Connected[SettlementPathNode.OppositeDirection(i)] = null;
            }
        }
        TestNodes2[x, z] = null;
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

            //SurroundByPath(r.X, r.Y, r.Width, r.Height, 2);
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


        return;
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


    public bool IsTileFree(int x, int z)
    {
        if (x < 0 || z < 0 || x >= TileSize || z >= TileSize)
            return false;

        /* foreach (Recti ri in BuildingPlots)
             if (ri.ContainsPoint(x, z))
                 return false;
                 */
        if (Tiles[x, z] != null)
            return false;
        return true;
    }
    public bool IsAreaFree(int x, int z, int width, int height, Tile ignoreTile=null)
    {
        if (x < 0 || z < 0)
            return false;
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
                if (z_ >= TileSize-1 || x_ >= TileSize-1)
                {
                    return false;
                }
                if(ignoreTile != null)
                {
                    if (Tiles[x_, z_] != null && Tiles[x_, z_] != ignoreTile)
                        return false;
                }else if (Tiles[x_, z_] != null)
                    return false;
            }
        }

        return true;
    }


}