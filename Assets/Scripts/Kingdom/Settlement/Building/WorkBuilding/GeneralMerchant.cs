using UnityEngine;
using UnityEditor;
[System.Serializable]
public class GeneralMerchant : WorkBuilding
{
    public static BuildingPlan BuildingPlan = new BuildingPlan("GeneralMerchant", 10, 15);
    public GeneralMerchant(int width, int height) : base(width, height)
    {
    }
}