using UnityEngine;
using UnityEditor;
using MarchingCubesProject;
using System.Collections.Generic;

public class TestMarch : MonoBehaviour
{


    private void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        int size = 16;
        float[] voxels = new float[size * size * size];
        MarchingCubes mc = new MarchingCubes(surface: 0.0f);
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    int idx = x + y * size + z * size * size;

                    if ( (x-8)* (x - 8)+ (z - 8) * (z - 8)+ (y - 8) * (y - 8) < 5 * 5)
                    {
                        voxels[idx] = -1;
                    }
                    else
                    {
                        voxels[idx] = 1;

                    }
                }
            }
        }
        List<Vector3> verts = new List<Vector3>();
        List<int> indices = new List<int>();
        mc.Generate(voxels, size, size, size, verts, indices);
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
    }
}