using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Collections.Generic;
using MarchingCubesProject;
public class MarchingCubesStoneWall : MonoBehaviour
{

    public int SIZE_XZ = 32;
    public int SIZE_Y = 4;
    public float Lifetime = 10;
    public float DestructionSpeed = 2;
    private Stopwatch Timer;
    
    private bool[,] WallPoints;

    private bool IsCollapsing;
    private MarchingCubes Marching;
    private Vec2i MidPos2;


    /// <summary>
    /// Called when the Marching cube wall is first created.
    /// Initiates the positions and arrays needed for the function to work.
    /// </summary>
    /// <param name="midpoint"></param>
    public void Initiate(Vector3 midpoint)
    {
        WallPoints = new bool[SIZE_XZ, SIZE_XZ];
        Timer = new Stopwatch();
        Timer.Start();
        Marching = new MarchingCubes(surface: 0);

        this.transform.position = midpoint - new Vector3(SIZE_XZ/2, 0, SIZE_XZ / 2);
        MidPos2 = Vec2i.FromVector3(midpoint);
    
    }
    //Attempts to build the wall 
    public void BuildWall(Vector3 position)
    {

        //If wall is collapsing, we cannot build on it
        if (IsCollapsing)
            return;
        //If we are adding to the wall or building on an existing point, we restart the destruction timer
        Timer.Restart();
        Vec2i pos2 = Vec2i.FromVector3(position) - MidPos2 + new Vec2i(SIZE_XZ / 2, SIZE_XZ / 2);
        //If the position is out of bounds, return
        if (pos2.x < 1 || pos2.x >= SIZE_XZ - 1 || pos2.z < 1 || pos2.z >= SIZE_XZ - 1)
            return;
        //If the wall point has already been created, return
        if (WallPoints[pos2.x,pos2.z])
            return;
        //Otherwise, add the point
        AddPoint(pos2.x, pos2.z);
    }

    private void Update()
    {
        if(Timer.ElapsedMilliseconds > Lifetime * 1000)
        {
            IsCollapsing = true;
        }
        if (IsCollapsing)
        {
            transform.position -= Vector3.up * Time.deltaTime * DestructionSpeed;
           
            if (transform.position.y < -8)
                Destroy(gameObject);
        }
    }

    private void AddPoint(int localX, int localZ)
    {
        WallPoints[localX, localZ] = true;

        March();
    }

    /// <summary>
    /// Runs the marching cube algorithm based on the 
    /// defined wall positions
    /// </summary>
    private void March()
    {
        //
        List<Vector3> verts = new List<Vector3>();
        List<int> indices = new List<int>();
        float[] Densities_ = new float[SIZE_XZ * SIZE_XZ * SIZE_Y];

        for(int x=0; x<SIZE_XZ; x++)
        {
            for (int z = 0; z < SIZE_XZ; z++)
            {
                for(int y=0; y<SIZE_Y-1; y++)
                {
                    int idx = x + y * SIZE_XZ + z * SIZE_Y * SIZE_XZ;
                    if (WallPoints[x, z])
                    {
                        Densities_[idx] = -1;
                    } 
                    else
                    {
                        Densities_[idx] = 1;
                    }

                }
                Densities_[x + (SIZE_Y - 1) * SIZE_XZ + z * SIZE_Y * SIZE_XZ] = 1;


            }
        }

        Marching.Generate(Densities_, SIZE_XZ, SIZE_Y, SIZE_XZ, verts, indices);
        gameObject.GetComponent<MeshFilter>().mesh = new Mesh();
        gameObject.GetComponent<MeshFilter>().mesh.SetVertices(verts);
        gameObject.GetComponent<MeshFilter>().mesh.triangles = indices.ToArray();
        gameObject.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        Vector2[] UV = new Vector2[verts.Count];
        float scaleFactor = 1;
        for (int index = 0; index < indices.Count; index += 3)
        {
            // Get the three vertices bounding this triangle.
            Vector3 v1 = verts[indices[index]];
            Vector3 v2 = verts[indices[index + 1]];
            Vector3 v3 = verts[indices[index + 2]];

            // Compute a vector perpendicular to the face.
            Vector3 normal = Vector3.Cross(v3 - v1, v2 - v1);

            // Form a rotation that points the z+ axis in this perpendicular direction.
            // Multiplying by the inverse will flatten the triangle into an xy plane.
            Quaternion rotation = Quaternion.Inverse(Quaternion.LookRotation(normal));

            // Assign the uvs, applying a scale factor to control the texture tiling.
            UV[indices[index]] = (Vector2)(rotation * v1) * scaleFactor;
            UV[indices[index + 1]] = (Vector2)(rotation * v2) * scaleFactor;
            UV[indices[index + 2]] = (Vector2)(rotation * v3) * scaleFactor;
        }


        gameObject.GetComponent<MeshFilter>().mesh.uv = UV;
        gameObject.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
    }



}