using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
public class LoadedChunk : MonoBehaviour
{


    public ChunkData Chunk { get; private set; }

    public WorldObject[,] LoadedWorldObjects { get; private set; }

    /// <summary>
    /// Called when component is added to object.
    /// Here we initiate some of the variables used for the loaded chunk, 
    /// such as adding the mesh and rendering stuff
    /// </summary>
    private void Awake()
    {
        /*gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = ResourceManager.GetMaterial("Chunk");
        gameObject.AddComponent<MeshCollider>();*/
    }


    public void SetChunkData(ChunkData chunk, ChunkData[] neighbors, bool forceLoad=false)
    {
        Debug.BeginDeepProfile("set_data");
        //Debug.BeginProfile("set_data");
        Chunk = chunk;
        transform.position = new Vector3(Chunk.X * World.ChunkSize, 0, Chunk.Z * World.ChunkSize);
        LoadedWorldObjects = new WorldObject[World.ChunkSize, World.ChunkSize];

        Debug.BeginDeepProfile("chunk_create_mesh");
        int waterDepth = -5;
        List<Vector3> verticies = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colours = new List<Color>();
        List<Vector3> normals = new List<Vector3>();
        int tri = 0;
        bool hasWater = false;
        ///Iterate every point in chunk, define terrain mesh based on tiles.
        #region mesh_gen
        for (int z=0; z<World.ChunkSize; z++)
        {
            for(int x=0; x<World.ChunkSize; x++)
            {
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                Tile t = Chunk.GetTile(x, z);
                Color c = t == null ? Tile.GRASS.GetColor() : t.GetColor();
                int y = 0;
                //Check if this tile is water, if so then height=-1
                if (t == Tile.WATER)
                {
                    hasWater = true;
                    y = waterDepth;
                }
                    

                verticies.Add(new Vector3(x, y, z));
                colours.Add(c);
                //If we are within non-extremes of chunk
                if (x<World.ChunkSize-1 && z < World.ChunkSize - 1)
                {
                    
                    verticies.Add(new Vector3(x, chunk.GetTile(x, z + 1)==Tile.WATER? waterDepth : 0, z + 1));
                    colours.Add(chunk.GetTile(x, z+1).GetColor());

                    verticies.Add(new Vector3(x + 1, chunk.GetTile(x+1, z + 1) == Tile.WATER ? waterDepth : 0, z + 1));
                    colours.Add(chunk.GetTile(x+1, z + 1).GetColor());

                    verticies.Add(new Vector3(x + 1, chunk.GetTile(x + 1, z) == Tile.WATER ? waterDepth : 0, z));
                    colours.Add(chunk.GetTile(x + 1, z).GetColor());

                }
                else
                {
                    //We are at an extreme
                    //Check x extreme, if we are not in x extreme then this must be Z extreme
                    if(x < World.ChunkSize - 1)
                    {
                        //Z EXTREME

                        Tile t1 = neighbors[0] != null ? neighbors[0].GetTile(x, 0) : null;
                        Tile t2 = neighbors[0] != null ? neighbors[0].GetTile(x+1, 0) : null;
                        Tile t3 = chunk.GetTile(x + 1, z);
                        if (t1 == null)
                        {
                            verticies.Add(new Vector3(x, y, z + 1));
                            colours.Add(c);
                        }else
                        {
                            if(t1 == Tile.WATER)
                                verticies.Add(new Vector3(x, waterDepth, z + 1));
                            else
                                verticies.Add(new Vector3(x, 0, z + 1));

                            colours.Add(t1.GetColor());
                        }

                        if (t2 == null)
                        {
                            verticies.Add(new Vector3(x+1, y, z + 1));
                            colours.Add(c);
                        }
                        else
                        {
                            if (t2 == Tile.WATER)
                                verticies.Add(new Vector3(x+1, waterDepth, z + 1));
                            else
                                verticies.Add(new Vector3(x+1, 0, z + 1));

                            colours.Add(t2.GetColor());
                        }
                        if(t3 == null)
                        {
                            verticies.Add(new Vector3(x + 1, y, z));
                            colours.Add(c);
                        }
                        else
                        {
                            if (t3 == Tile.WATER)
                                verticies.Add(new Vector3(x + 1, waterDepth, z));
                            else
                                verticies.Add(new Vector3(x + 1, 0, z));

                            colours.Add(t3.GetColor());
                        }
                      

                    }
                    else if(z < World.ChunkSize - 1)
                    {
                        //X EXTREME
                        Tile t1 = chunk.GetTile(x, z+1);
                        Tile t2 = neighbors[2] != null ? neighbors[2].GetTile(0, z+1) : null;
                        Tile t3 = neighbors[2] != null ? neighbors[2].GetTile(0, z) : null;
                        if (t1 == null)
                        {
                            verticies.Add(new Vector3(x, y, z + 1));
                            colours.Add(c);
                        }
                        else
                        {
                            if (t1 == Tile.WATER)
                                verticies.Add(new Vector3(x, waterDepth, z + 1));
                            else
                                verticies.Add(new Vector3(x, 0, z + 1));

                            colours.Add(t1.GetColor());
                            //colours.Add(Color.red);

                        }

                        if (t2 == null)
                        {
                            verticies.Add(new Vector3(x + 1, y, z + 1));
                            colours.Add(c);
                        }
                        else
                        {
                            if (t2 == Tile.WATER)
                                verticies.Add(new Vector3(x + 1, waterDepth, z + 1));
                            else
                                verticies.Add(new Vector3(x + 1, 0, z + 1));
                            //colours.Add(Color.magenta);
                            colours.Add(t2.GetColor());
                            //colours.Add(Color.yellow);
                        }
                        if (t3 == null)
                        {
                            verticies.Add(new Vector3(x + 1, y, z));
                            colours.Add(c);
                        }
                        else
                        {
                            if (t3 == Tile.WATER)
                                verticies.Add(new Vector3(x + 1, waterDepth, z));
                            else
                                verticies.Add(new Vector3(x + 1, 0, z));

                            colours.Add(t3.GetColor());
                            
                        }
                    }
                    else
                    {
                        //BOTH EXTREME
                        Tile t1 = neighbors[0] != null ? neighbors[0].GetTile(x, 0) : null;
                        Tile t2 = neighbors[1] != null ? neighbors[1].GetTile(0, 0) : null;
                        Tile t3 = neighbors[2] != null ? neighbors[2].GetTile(0, z) : null;
                        if (t1 == null)
                        {
                            verticies.Add(new Vector3(x, y, z + 1));
                            colours.Add(c);
                        }
                        else
                        {
                            if (t1 == Tile.WATER)
                                verticies.Add(new Vector3(x, waterDepth, z + 1));
                            else
                                verticies.Add(new Vector3(x, 0, z + 1));

                            colours.Add(t1.GetColor());
                        }

                        if (t2 == null)
                        {
                            verticies.Add(new Vector3(x + 1, y, z + 1));
                            colours.Add(c);
                        }
                        else
                        {
                            if (t2 == Tile.WATER)
                                verticies.Add(new Vector3(x + 1, waterDepth, z + 1));
                            else
                                verticies.Add(new Vector3(x + 1, 0, z + 1));

                            colours.Add(t2.GetColor());
                        }
                        if (t3 == null)
                        {
                            verticies.Add(new Vector3(x + 1, y, z));
                            colours.Add(c);
                        }
                        else
                        {
                            if (t3 == Tile.WATER)
                                verticies.Add(new Vector3(x + 1, waterDepth, z ));
                            else
                                verticies.Add(new Vector3(x + 1, 0, z));

                            colours.Add(t3.GetColor());
                        }

                    }
                }

               
                triangles.Add(tri);
                triangles.Add(tri+1);
                triangles.Add(tri+2);
                triangles.Add(tri);
                triangles.Add(tri + 2);
                triangles.Add(tri + 3);
                tri += 4;




            }
        }

        if (!hasWater)
        {
            foreach (Vector3 v in verticies)
            {
                if (v.y != 0)
                {
                    hasWater = true;
                    break;
                }
            }
        }
        Debug.EndDeepProfile("chunk_create_mesh");
        Debug.BeginDeepProfile("chunk_set_mesh");
        MeshFilter meshFilt = GetComponent<MeshFilter>();

        meshFilt.mesh = new Mesh();
        meshFilt.mesh.vertices = verticies.ToArray();
        meshFilt.mesh.triangles = triangles.ToArray();
        meshFilt.mesh.colors = colours.ToArray();
        meshFilt.mesh.normals = normals.ToArray();
        GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        gameObject.GetComponent<MeshRenderer>().material = ResourceManager.GetMaterial("Chunk");
        #endregion
        
        Vector3[] colVerts = new Vector3[] {new Vector3(0,0,0), new Vector3(0,0,World.ChunkSize),
            new Vector3(World.ChunkSize, 0, World.ChunkSize), new Vector3(World.ChunkSize, 0, 0)};
        int[] colTris = new int[] { 0, 1, 2, 0, 2, 3 };
        Mesh colMesh = GetComponent<MeshCollider>().sharedMesh = new Mesh();

        if (hasWater)
        {
            colMesh.vertices = GetComponent<MeshFilter>().mesh.vertices;
            colMesh.triangles = GetComponent<MeshFilter>().mesh.triangles;

        }
        else
        {
            colMesh.vertices = colVerts;
            colMesh.triangles = colTris;
        }
        this.name = this.name + chunk.DEBUG;
        Debug.EndDeepProfile("chunk_set_mesh");

        Vec2i chunkWorldPos = new Vec2i(chunk.X, chunk.Z) * World.ChunkSize;
        if (Chunk.Objects == null)
        {
            Debug.EndDeepProfile("set_data");
            return;


        }

        Debug.BeginDeepProfile("chunk_set_obj");
        for (int x=0; x<World.ChunkSize; x++)
        {
            for(int z=0; z<World.ChunkSize; z++)
            {

                WorldObjectData wObj = Chunk.GetObject(x, z);
                if(wObj != null)
                {
                  
                        wObj.SetPosition(new Vec2i(chunk.X * World.ChunkSize + x, chunk.Z * World.ChunkSize + z));
                        //Debug.BeginDeepProfile("CreateWorldObj");
                        //objs.Add(wObj);
                        WorldObject loaded = wObj.CreateWorldObject(transform);
                        LoadedWorldObjects[x, z] = loaded;
                }
            }
        }
        Debug.EndDeepProfile("chunk_set_obj");
        Debug.EndDeepProfile("set_data");

        //Debug.EndProfile();
    }


}