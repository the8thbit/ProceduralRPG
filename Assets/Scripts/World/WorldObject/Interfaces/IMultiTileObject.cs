using UnityEngine;
using UnityEditor;

public interface IMultiTileObject
{
    /// <summary>
    /// Used to find all the children of a multi child object.
    /// This method should check if the children have already been generated.
    /// If they have not, it should generate them.
    /// </summary>
    /// <returns></returns>
    IMultiTileObjectChild[,] GetChildren();

}

public interface IMultiTileObjectChild
{
    IMultiTileObject Getparent();
}