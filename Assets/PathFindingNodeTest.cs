using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PathFindingNodeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public float[,] map;
    public int x, z;
    public SettlementPathNode spn;
    // Update is called once per frame
    public bool shouldShow;

    private void OnDrawGizmos()
    {
        Color old = Gizmos.color;
        Gizmos.color = Color.yellow;
        if (shouldShow)
        {

            if (x > 0 && map[x - 1, z] > 0)
            {
                Gizmos.DrawCube(toPos(x - 1, z), Vector3.one*2);
            }
            if (z > 0 && map[x , z-1] > 0)
            {
                Gizmos.DrawCube(toPos(x, z - 1), Vector3.one * 2);
            }
            if (x < map.GetLength(0)-1 && map[x+1, z] > 0)
            {
                Gizmos.DrawCube(toPos(x+1, z), Vector3.one * 2);
            }
            if (z < map.GetLength(0) - 1 && map[x, z+1] > 0)
            {
                Gizmos.DrawCube(toPos(x, z+1), Vector3.one * 2);
            }
            
        }
        Gizmos.color = old;
    }
    private Vector3 toPos(int x, int z)
    {
        return Vec2i.ToVector3(GameManager.TestSettle.BaseCoord + new Vec2i(x, z) * World.ChunkSize);
    }
}
