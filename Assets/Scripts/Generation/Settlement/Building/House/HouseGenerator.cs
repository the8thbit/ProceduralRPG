using UnityEngine;
using UnityEditor;

public class HouseGenerator
{

    public static House GenerateShack(House house)
    {
        BuildingGenerator.GenerateWallsFloorAndEntrance(house.Width, house.Height, house.BuildingObjects, house.BuildingTiles, 0, BuildingStyle.wood);
        //BuildingGenerator.AddObject(house.BuildingObjects, WorldObject.BED, 2, 1);

        return house;
    }



}