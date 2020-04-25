using UnityEngine;
using UnityEditor;

public class BuildingGenerator
{

    public const int NORTH_ENTRANCE = 0;
    public const int EAST_ENTRANCE = 1;
    public const int SOUTH_ENTRANCE = 2;
    public const int WEST_ENTRANCE = 3;

    /// <summary>
    /// Takes a building plan and generates a full building from it.
    /// TODO - Currently messy, fix
    /// </summary>
    /// <param name="plan"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="entrance"></param>
    /// <returns></returns>
    public static Building CreateBuilding(BuildingPlan plan, int width=-1, int height=-1, int entrance = 0)
    {
        if (width == -1)
            width = MiscMaths.RandomRange(plan.MinSize, plan.MaxSize);
        else if (width < plan.MinSize)
            width = plan.MinSize;
        else if (width > plan.MaxSize)
            width = plan.MaxSize;

        if (height == -1)
            height = MiscMaths.RandomRange(plan.MinSize, plan.MaxSize);
        else if (height < plan.MinSize)
            height = plan.MinSize;
        else if (height > plan.MaxSize)
            height = plan.MaxSize;

        Tile[,] buildingBase = new Tile[width, height];
        WorldObjectData[,] buildingObjects = new WorldObjectData[width, height];

        if(plan == Building.HOUSE)
        {
            House h = new House(width, height);
            h.SetBuilding(buildingBase, buildingObjects);
            return GenerateHouse(h, BuildingStyle.stone);
        }
        if(plan == Building.BLACKSMITH)
        {
            Blacksmith b = new Blacksmith(width, height);
            b.SetBuilding(buildingBase, buildingObjects);
            return GenerateBlacksmith(b, BuildingStyle.stone);
        }
        /*
        if(plan == Building.MARKET)
        {
            MarketPlace market = new MarketPlace(width, height);
            market.SetBuilding(buildingBase, buildingObjects);
            return GenerateMarket(market, BuildingStyle.stone);
        }
        if(plan == Building.BARACKSCITY)
        {
            Baracks bar = new Baracks(width, height);
            bar.SetBuilding(buildingBase, buildingObjects);
            return GenerateCityBaracks(bar);
        }
        if (plan == Building.BARACKSCITY || plan == Building.BARACKSTOWN)
        {
            
        }*/

        //Default for now
        House ht = new House(width, height);
        ht.SetBuilding(buildingBase, buildingObjects);
        return GenerateHouse(ht, BuildingStyle.stone);
        
    }

    /// <summary>
    /// Generates the basic walls, floor, and entrance for any building.
    /// Picks the exact position of entrance, and returns it
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="buildingObjects">Building objects to be filled with walls</param>
    /// <param name="buildTiles"></param>
    /// <param name="entranceID"></param>
    /// <param name="style"></param>
    /// <returns></returns>
    public static Vec2i GenerateWallsFloorAndEntrance(int width, int height, WorldObjectData[,] buildingObjects, 
        Tile[,] buildTiles, int entranceID, BuildingStyle style, int entraceDis=-1, WorldObjectData wallType =null, Tile tileType=null)
    {

        int entWidth = 2;
        if(entraceDis == -1)
        {
            if (entranceID == NORTH_ENTRANCE || entranceID == SOUTH_ENTRANCE)
                entraceDis = MiscMaths.RandomRange(1, width - 1);
            else
                entraceDis = MiscMaths.RandomRange(1, height - 1);
        }

        //Decide the position of the entrance
        Vec2i entrance = null;
        if (entranceID == NORTH_ENTRANCE)
            entrance = new Vec2i(entraceDis, height-1);
        else if (entranceID == SOUTH_ENTRANCE)
            entrance = new Vec2i(entraceDis, 0);
        else if (entranceID == EAST_ENTRANCE)
            entrance = new Vec2i(width-1, entraceDis);
        else if (entranceID == WEST_ENTRANCE)
            entrance = new Vec2i(0, entraceDis);
        
        
        //Assign correct wall type if none given
        if(wallType == null)
        {
            switch (style)
            {
                case BuildingStyle.stone:
                    wallType = new BrickWall(new Vec2i(0,0));
                    break;
                case BuildingStyle.wood:
                    //TODO - Add
                    wallType = new BrickWall(new Vec2i(0, 0));
                    break;
            }
        }//Asign correct tile type if none given
        if(tileType == null)
        {
            switch (style)
            {
                case BuildingStyle.stone:
                    tileType = Tile.STONE_FLOOR;
                    break;
                case BuildingStyle.wood:
                    tileType = Tile.WOOD_FLOOR;
                    break;
            }
        }
        //Iterate all points
        for(int x=0; x<width; x++)
        {
            for(int z=0; z < height; z++)
            {
                //Asign tile
                buildTiles[x, z] = tileType;
                if(x==0 || x==width-1 || z==0 || z == height - 1)
                {
                    //Asign wall if no entrance
                    if (!(entrance.x == x & entrance.z == z))
                    {
                        if (x == 3)
                        {
                            buildingObjects[x, z] = new BuildingWall(new Vec2i(0, 0), "brick", 5,
                                new GlassWindow(new Vec2i(0, 0), new Vec2i(0, 1)), 2);
                        }
                        else
                        {
                            buildingObjects[x, z] = new BuildingWall(new Vec2i(0,-1), "brick", 5);
                        }
                        
                    }
                        
                }
            }
        }
        //(buildingObjects[0, 0] as BuildingWall).AddRoof(new Roof(new Vec2i(0, 0), new Vec2i(width, height)));

        return entrance;
    }

    /// <summary>
    /// Adds given object to the array of building objects.
    /// Used to make easy placement of multicell objects.
    /// </summary>
    /// <param name="current"></param>
    /// <param name="nObj"></param>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public static void AddObject(Building building, WorldObjectData nObj, int x, int z, bool nInstance=false)
    {
        building.BuildingObjects[x, z] = nObj;
        building.AddObjectReference(nObj);
        /*
        if (nObj.HasMetaData && nObj.MetaData.IsMulticell)
        {
            int w = nObj.MetaData.MultiCellWidth;
            int h = nObj.MetaData.MultiCellHeight;
            for(int x_=0; x_<w; x_++)
            {
                for(int z_=0; z_<h; z_++)
                {
                    if (x_ == 0 && z_ == 0)
                        continue;
                    WorldObject multSub = nInstance?WorldObject.CreateNewInstance(WorldObject.MULTI_TILE_CELL):nObj;
                    multSub.MetaData.SetParent(nObj);
                    current[x + x_, z + z_] = multSub;
                }
            }
        }*/
    }

    public static Vec2i AddEntrance(Building b, int entranceSide, int disp = -1)
    {

        if(disp == -1)
        {
            //If entrance is facing z axis, displacement in in x
            if (entranceSide == NORTH_ENTRANCE || entranceSide == SOUTH_ENTRANCE)
                disp = MiscMaths.RandomRange(1, b.Width - 1);
            else
                disp = MiscMaths.RandomRange(1, b.Height - 1);
        }
        
        Vec2i entrance = null;
        if (entranceSide == NORTH_ENTRANCE)
            entrance = new Vec2i(disp, b.Height - 1);
        else if (entranceSide == SOUTH_ENTRANCE)
            entrance = new Vec2i(disp, 0);
        else if (entranceSide == EAST_ENTRANCE)
            entrance = new Vec2i(b.Width - 1, disp);
        else if (entranceSide == WEST_ENTRANCE)
            entrance = new Vec2i(0, disp);
        b.BuildingObjects[entrance.x, entrance.z] = null; //TODO - Set to door of correct type
        return entrance;
    }

    public static House GenerateHouse(Building building, BuildingStyle style, WorldObject wallType = null, Tile tileType = null)
    {
        int width = building.Width;
        int height = building.Height;
        GenerateWallsFloorAndEntrance(width, height, building.BuildingObjects, building.BuildingTiles, 0, BuildingStyle.stone, tileType:Tile.TEST_RED);
        //AddObject(building.BuildingObjects, WorldObject.BED, 2, 3);
        
        return building as House;
    }
    
    public static Blacksmith GenerateBlacksmith(Blacksmith building, BuildingStyle style = BuildingStyle.stone)
    {
        int width = building.Width;
        int height = building.Height;
        GenerateWallsFloorAndEntrance(width, height, building.BuildingObjects, building.BuildingTiles, 0, style, tileType:Tile.TEST_MAGENTA);

        WorkEquiptmentData anvil = new Anvil(new Vec2i(1, 3));
        WorkEquiptmentData forge = new Anvil(new Vec2i(4,4));
        AddObject(building, anvil, 1,3);
        AddObject(building, forge, 4, 4);

        NPCJob[] jobs = new NPCJob[] { new NPCJobBlackSmith(building), 
                                       new NPCJobBlackSmith(building), 
                                       new NPCJobBlackSmith(building) };

        building.SetWorkBuildingData(new WorkBuildingData(jobs));

        //building.WorkEquiptment.Add(new WorkEquiptmentPlacement(anvil, new Vec2i(1, 3)));
        //building.WorkEquiptment.Add(new WorkEquiptmentPlacement(forge, new Vec2i(4, 4)));
        return building;
    }



    /// <summary>
    /// Creates a castle, size must be 48x48
    /// </summary>
    /// <returns></returns>
    public static Castle GenerateCastle(int size)
    {
        Tile[,] buildingBase = new Tile[size, size];
        WorldObjectData[,] buildingObjects = new WorldObjectData[size, size];
        Castle c = new Castle(size, size);
        c.SetBuilding(buildingBase, buildingObjects);
        //c.Entrances.Add(new BuildingEntrance(new Vec2i(1, size / 2 - 2), new Vec2i(1, size / 2 + 2)));
        Vec2i entr = GenerateWallsFloorAndEntrance(size, size, c.BuildingObjects, c.BuildingTiles, 0, BuildingStyle.stone, entraceDis:size/2);
        
        
        AddEntrance(c, 0, entr.x - 1);
        AddEntrance(c, 0, entr.x - 2);

        AddEntrance(c, 0, entr.x + 1);
        AddEntrance(c, 0, entr.x + 2);

        return c;
    }

}