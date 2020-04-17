using UnityEngine;
using UnityEditor;

/// <summary>
/// Used for world objects that are procedually generated 
/// </summary>
public interface IProcedualGeneratedObject
{

    void SetRandom(GenerationRandom ran);
}